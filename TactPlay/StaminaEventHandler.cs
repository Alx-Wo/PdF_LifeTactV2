using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TactPlay.Pattern;

namespace TactPlay
{
    class StaminaEventHandler
    {
        private double currentStamina = 1;
        private double maxStamina = 1;
        private StaminaState[] staminaHistory = new StaminaState[50];
        private bool useRepeatablePattern = true;
        private RepeatablePattern repeatablePattern;

        private static readonly double WALK_STAMINA_CHANGE_PER_SECOND = 0.1; //Walking drains 0,1 stamina points per second
        private static readonly double SPRINT_STAMINA_CHANGE_PER_SECOND = 1.0; //Sprinting drains 1 stamina points per second
        private static readonly double STAND_STAMINA_PERCENTAGE_CHANGE_PER_SECOND = 1.0 / 30; //Standing regenerates 1/30.0 of maxStamina per second

        private Timer timer;
        private bool enabled = false;
        private long lastNotification = 0;
        private long dialogOpenedTime = 0;
        private double lastStamina = 0;
        private double lastStaminaBeforeDialog = 0;

        public StaminaEventHandler()
        {
            for (int i = 0; i < staminaHistory.Length; i++)
            {
                staminaHistory[i] = new StaminaState(Environment.TickCount, currentStamina, maxStamina);
            }


            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 250;
            timer.Start();
        }

        public void OnDialogOpened()
        {
            dialogOpenedTime = Environment.TickCount;
            lastStaminaBeforeDialog = lastStamina;
            Logger.WriteLogLine("Stamina.log", "StaminaOpened");
        }

        public void OnDialogClosed(String userInput)
        {
            Logger.WriteLogLine("Stamina.log", "StaminaClosed\t" +
                "Eingabe: \t" + userInput + "\t" +
                "AktuellerWert: \t" + ((currentStamina / maxStamina) * 100) + "\t" +
                "LetzterWertVorOpened: \t" + (lastStaminaBeforeDialog * 100) + "\t" +
                "ZeitZwischenOpenedUndClosed: \t" + (Environment.TickCount - dialogOpenedTime));
        }

        private void ShiftHistoryArrayRight()
        {
            for (int i = staminaHistory.Length - 1; i > 0; i--)
            {
                staminaHistory[i] = staminaHistory[i - 1];
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (!enabled)
            {
                return;
            }
            ShiftHistoryArrayRight();
            staminaHistory[0] = new StaminaState(Environment.TickCount, currentStamina, maxStamina);
            PlayVibraion();
        }

        private void PlayVibraion()
        {
            if (useRepeatablePattern)
            {
                lastStamina = currentStamina / maxStamina;
                IPattern newPattern = Utils.STAMINA_PATTERN_STUDY_2(lastStamina);
                if (repeatablePattern == null || !repeatablePattern.IsRepeating())
                {
                    repeatablePattern = new RepeatablePattern(newPattern);
                    Main.vibrationDevice.PlayPattern(repeatablePattern);
                }
                else
                {
                    repeatablePattern.UpdatePattern(newPattern);
                }
            }
            else
            {
                if (Environment.TickCount > lastNotification + 3000)
                {
                    double change = GetStaminaChangePerSecond(1500);
                    double pctChange = GetStaminaPercentageChangePerSecond(1500);
                    if (pctChange < 0)
                    {
                        //stamina drain
                        if (Math.Abs(WALK_STAMINA_CHANGE_PER_SECOND - Math.Abs(change)) < WALK_STAMINA_CHANGE_PER_SECOND * 0.3)
                        {
                            //walking
                        }
                        else if (Math.Abs(SPRINT_STAMINA_CHANGE_PER_SECOND - Math.Abs(change)) < SPRINT_STAMINA_CHANGE_PER_SECOND * 0.3)
                        {
                            //sprinting
                            lastNotification = Environment.TickCount;
                            Main.vibrationDevice.PlayPattern(Utils.STAMINA_PATTERN_5(-1));
                        }

                    }
                    else
                    {
                        //stamina regeneration or no change
                        if (Math.Abs(STAND_STAMINA_PERCENTAGE_CHANGE_PER_SECOND - Math.Abs(pctChange)) < STAND_STAMINA_PERCENTAGE_CHANGE_PER_SECOND * 0.3)
                        {
                            //regeneration
                            lastNotification = Environment.TickCount;
                            Main.vibrationDevice.PlayPattern(Utils.STAMINA_PATTERN_5(1));
                        }
                    }
                }
            }
        }

        private double GetStaminaChangePerSecond(long timeWindow)
        {
            long currentTime = Environment.TickCount;
            long targetTime = currentTime - timeWindow;
            for (int i = 0; i < staminaHistory.Length; i++)
            {
                if (staminaHistory[i].Time < targetTime)
                {
                    return (staminaHistory[0].CurrentStamina - staminaHistory[i].CurrentStamina) / (((double)staminaHistory[0].Time - staminaHistory[i].Time) / 1000);
                }
            }
            return 0;
        }

        private double GetStaminaPercentageChangePerSecond(long timeWindow)
        {
            long currentTime = Environment.TickCount;
            long targetTime = currentTime - timeWindow;
            for (int i = 0; i < staminaHistory.Length; i++)
            {
                if (staminaHistory[i].Time < targetTime)
                {
                    return ((staminaHistory[0].CurrentStamina / staminaHistory[0].MaxStamina) - (staminaHistory[i].CurrentStamina / staminaHistory[i].MaxStamina)) / (((double)currentTime - staminaHistory[i].Time) / 1000);
                }
            }
            return 0;
        }

        public void EventTimer(double currentStamina, double maxStamina)
        {
            if (!enabled)
            {
                return;
            }
            this.currentStamina = currentStamina;
            this.maxStamina = maxStamina;
        }

        public void Enable()
        {
            enabled = true;
            timer.Start();
        }

        public void Disable()
        {
            enabled = false;
            timer.Stop();
            if (repeatablePattern != null)
            {
                repeatablePattern.Stop();
            }
            repeatablePattern = null;
        }

        private class StaminaState
        {
            private readonly double currentStamina, maxStamina;
            private readonly long time;
            public StaminaState(long time, double currentStamina, double maxStamina)
            {
                this.time = time;
                this.currentStamina = currentStamina;
                this.maxStamina = maxStamina;
            }

            public double CurrentStamina => currentStamina;

            public double MaxStamina => maxStamina;

            public long Time => time;
        }
    }
}
