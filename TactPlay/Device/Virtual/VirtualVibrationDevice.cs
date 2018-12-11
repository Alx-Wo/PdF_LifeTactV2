using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using TactPlay.Pattern;

namespace TactPlay.Device.Virtual
{
    public partial class VirtualVibrationDevice : Form, IVibrationDevice
    {
        private int numberOfMotors;
        private MotorThread[] motorThreads;
        private GraphPanelMotor[] graphPanels = new GraphPanelMotor[NUMBER_OF_MOTORS];
        private List<RepeatPattern> repeatablePatternList = new List<RepeatPattern>();
        private readonly object[] motorLocks;
        private readonly object syncLock = new object();
        private static readonly int NUMBER_OF_MOTORS = 8;

        public VirtualVibrationDevice()
        {
            InitializeComponent();


            this.numberOfMotors = NUMBER_OF_MOTORS;
            motorThreads = new MotorThread[numberOfMotors];
            motorLocks = new object[numberOfMotors];

            int panelHeight = ClientSize.Height / NUMBER_OF_MOTORS;
            int panelWidth = ClientSize.Width;

            for (int i = 0; i < NUMBER_OF_MOTORS; i++)
            {
                GraphPanelMotor panel = new GraphPanelMotor();
                panel.Location = new Point(0, panelHeight * i);
                panel.Size = new Size(panelWidth, panelHeight);
                graphPanels[i] = (panel);
                this.Controls.Add(panel);
            }
            System.Timers.Timer timer;
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 5;
            timer.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            lock (syncLock)
            {
                List<RepeatPattern> removeList = new List<RepeatPattern>();
                foreach (RepeatPattern repeatP in repeatablePatternList)
                {
                    if (repeatP.pattern.IsRepeating() && repeatP.endTime <= Environment.TickCount)
                    {
                        long startTime = Environment.TickCount;
                        List<SimplePattern> simplePatternList = repeatP.pattern.GetSimplePatterns();
                        repeatP.endTime = startTime + Utils.GetMaxDuration(simplePatternList);
                        foreach (SimplePattern sp in simplePatternList)
                        {
                            StartPatternOnMotor(sp.GetMotor(), sp);
                        }
                    }
                    if (!repeatP.pattern.IsRepeating())
                    {
                        removeList.Add(repeatP);
                    }
                }
                foreach (RepeatPattern repeatP in removeList)
                {
                    repeatablePatternList.Remove(repeatP);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g;

            g = e.Graphics;

            Pen myPen = new Pen(Color.Red);
            myPen.Width = 30;
            g.DrawLine(myPen, 30, 30, 45, 65);

            g.DrawLine(myPen, 1, 1, 45, 65);
        }

        public void PlayPattern(IPattern pattern)
        {
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
                repeatablePatternList.Add(new RepeatPattern(repeatablePattern, Environment.TickCount + Utils.GetMaxDuration(simplePatternList)));
                foreach (SimplePattern sp in simplePatternList)
                {
                    StartPatternOnMotor(sp.GetMotor(), sp);
                }
            }
            else
            {

                foreach (SimplePattern p in pattern.GetSimplePatterns())
                {
                    int motor = p.GetMotor();
                    if (motor >= numberOfMotors || motor < 0)
                    {
                        Console.Error.WriteLine("Invalid motor id: " + motor + ". Vibration device has " + numberOfMotors + " motorThreads.");
                    }
                    StartPatternOnMotor(motor, p);
                }
            }
        }

        private void StartPatternOnMotor(int motorNumber, SimplePattern pattern)
        {
            lock (syncLock)
            {
                if (motorThreads[motorNumber] == null)
                {
                    motorThreads[motorNumber] = new MotorThread(graphPanels[motorNumber]);
                }
            }
            MotorThread thread = motorThreads[motorNumber];
            lock (motorThreads[motorNumber])
            {
                thread.StartPattern(pattern);
            }
        }

        public int GetNumberOfMotors()
        {
            return numberOfMotors;
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
