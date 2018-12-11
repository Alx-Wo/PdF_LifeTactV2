using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TactPlay.Connection;
using TactPlay.Pattern;

namespace TactPlay.Device
{
    class VibrationDevice : IVibrationDevice
    {
        private static readonly int NUMBER_OF_MOTORS = 8;
        private static readonly int MIN_DELAY_BETWEEN_SENDS = 35;
        private long lastSend = 0;
        private ITactPlayConnection tactPlayConnection;
        private List<VibrationAction> actions = new List<VibrationAction>();
        private List<RepeatPattern> repeatablePatternList = new List<RepeatPattern>();
        private object lockObject = new object();
        private int strengthOffset = 100;
        private byte[] lastStrengthArray = new byte[NUMBER_OF_MOTORS + 1];
        private Thread timerThread;
        private bool keepTimerThreadRunning = true;


        public VibrationDevice(ITactPlayConnection tactPlayConnection)
        {
            this.tactPlayConnection = tactPlayConnection;
            timerThread = new Thread(() =>
            {
                var interval = new TimeSpan(0, 0, 0, 0, 5);
                var nextTick = DateTime.Now + interval;
                while (keepTimerThreadRunning)
                {
                    while (DateTime.Now < nextTick)
                    {
                        Thread.Sleep(Math.Max((nextTick - DateTime.Now).Milliseconds, 0));
                    }
                    OnTimedEvent();
                    nextTick = DateTime.Now + interval;
                }
            });
            timerThread.Start();
        }

        private static long getTimeMilliseconds()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void OnTimedEvent()
        {
            lock (lockObject)
            {
                List<RepeatPattern> removeList = new List<RepeatPattern>();
                foreach (RepeatPattern repeatP in repeatablePatternList)
                {
                    if (repeatP.pattern.IsRepeating() && repeatP.endTime <= getTimeMilliseconds())
                    {
                        long startTime = getTimeMilliseconds();
                        List<SimplePattern> simplePatternList = repeatP.pattern.GetSimplePatterns();
                        repeatP.endTime = startTime + Pattern.Utils.GetMaxDuration(simplePatternList);
                        PlaySimplePatterns(simplePatternList, startTime, false);
                    }
                    if (!repeatP.pattern.IsRepeating() && repeatP.endTime <= getTimeMilliseconds())
                    {
                        //stop Motors
                        foreach (SimplePattern sp in repeatP.pattern.GetSimplePatterns())
                        {
                            actions.Add(new VibrationAction(sp.GetMotor(), repeatP.endTime, 0));
                        }
                        removeList.Add(repeatP);
                    }
                }
                foreach (RepeatPattern repeatP in removeList)
                {
                    repeatablePatternList.Remove(repeatP);
                }
            }

            lock (lockObject)
            {
                if (getTimeMilliseconds() < lastSend + MIN_DELAY_BETWEEN_SENDS)
                {
                    return;
                }
                long currentTime = getTimeMilliseconds();
                List<VibrationAction> tempActions = new List<VibrationAction>();
                foreach (VibrationAction action in actions)
                {
                    if (action.actionTime <= currentTime)
                    {
                        tempActions.Add(action);
                    }
                }
                if (tempActions.Count == 0)
                {
                    return; //Nothing to do
                }

                foreach (VibrationAction action in tempActions)
                {
                    actions.Remove(action); // Remove the action from the actions list
                }
                List<VibrationAction> actionsToDo = new List<VibrationAction>();
                for (int i = 0; i < NUMBER_OF_MOTORS; i++)
                {
                    // Look for actions in tempActions that have the same motor and only use the newest
                    List<VibrationAction> actionsByMotorID = GetVibrationActionByMotor(tempActions, i);
                    VibrationAction actionToAdd = actionsByMotorID.FirstOrDefault();
                    foreach (VibrationAction action in actionsByMotorID)
                    {
                        if (actionToAdd.actionTime < action.actionTime)
                        {
                            actionToAdd = action;
                        }
                    }
                    if (actionToAdd != null)
                    {
                        actionsToDo.Add(actionToAdd);
                    }
                }

                byte[] strengthArray = lastStrengthArray;
                strengthArray[0] = 0xFF; //Add 0xFF as first byte to indicate control over all motors
                foreach (VibrationAction action in actionsToDo)
                {
                    actions.Remove(action);
                    //Console.WriteLine("Performing action \t" + (getTimeMilliseconds() - action.actionTime) + "ms \tafter scheduled time. " + "Last Action before \t" + (getTimeMilliseconds() - lastSend) + "ms. ");
                    strengthArray[action.motorID + 1] = action.strength;
                }
                if (getTimeMilliseconds() < lastSend + MIN_DELAY_BETWEEN_SENDS + 10)
                {
                    //Console.WriteLine("Sending after " + (getTimeMilliseconds() - lastSend) + "ms");
                }

                //Console.WriteLine(getTimeMilliseconds() + " Performing change: " + Main.ByteArrayToString(strengthArray));
                tactPlayConnection.SendBytes(strengthArray);
                lastSend = getTimeMilliseconds();
            }
        }

        private List<VibrationAction> GetVibrationActionByMotor(List<VibrationAction> list, int motor)
        {
            List<VibrationAction> vibrationActions = new List<VibrationAction>();
            foreach (VibrationAction action in list)
            {
                if (action.motorID.Equals(motor))
                {
                    vibrationActions.Add(action);
                }
            }
            return vibrationActions;
        }

        public void PlayPattern(IPattern pattern)
        {
            lock (lockObject)
            {
                long startTime = getTimeMilliseconds();
                if (pattern.GetType() == typeof(RepeatablePattern))
                {
                    foreach (RepeatPattern repeatP in repeatablePatternList)
                    {
                        if (repeatP.pattern.ID == ((RepeatablePattern)pattern).ID)
                        {
                            return; //Pattern already present
                        }
                    }
                    RepeatablePattern repeatablePattern = (RepeatablePattern)pattern;
                    List<SimplePattern> simplePatternList = repeatablePattern.GetSimplePatterns();
                    repeatablePatternList.Add(new RepeatPattern(repeatablePattern, startTime + Pattern.Utils.GetMaxDuration(simplePatternList)));
                    PlaySimplePatterns(simplePatternList, startTime, false);
                }
                else
                {
                    PlaySimplePatterns(pattern.GetSimplePatterns(), startTime, true);
                }
            }
        }

        private void PlaySimplePatterns(List<SimplePattern> patternList, long startTime, bool addMotorStop)
        {
            foreach (SimplePattern p in patternList)
            {
                int[] timings = p.GetTimings();
                double[] amplitudes = p.GetAmplitudes();
                int timeElapsed = 0;
                for (int i = 0; i < timings.Length; i++)
                {
                    actions.Add(new VibrationAction(p.GetMotor(), startTime + timeElapsed, GetStrengthFromDouble(amplitudes[i])));
                    timeElapsed += timings[i];
                }
                if (addMotorStop)
                {
                    actions.Add(new VibrationAction(p.GetMotor(), startTime + timeElapsed, 0));
                }
            }
        }

        private byte GetStrengthFromDouble(double strength)
        {
            if (strength == 0.0)
            {
                return Convert.ToByte(0);
            }
            return Convert.ToByte(Math.Floor(strength * (255 - strengthOffset)) + strengthOffset);
        }

        public int GetNumberOfMotors()
        {
            return NUMBER_OF_MOTORS;
        }

        private class VibrationAction
        {
            public int motorID;
            public long actionTime;
            public byte strength;

            public VibrationAction(int motorID, long actionTime, byte strength)
            {
                this.motorID = motorID;
                this.actionTime = actionTime;
                this.strength = strength;
            }
        }
        private class RepeatPattern
        {
            public RepeatablePattern pattern;
            public long endTime;

            public RepeatPattern(RepeatablePattern pattern, long endTime)
            {
                this.pattern = pattern;
                this.endTime = endTime;
            }
        }
    }
}
