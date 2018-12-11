using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TactPlay.Pattern;

namespace TactPlay.Device
{
    class MotorThread
    {
        private SimplePattern pattern;
        private IMotor motor;
        private AutoResetEvent autoReset;
        private readonly object syncLock = new object();
        private Thread t;

        public MotorThread(IMotor motor)
        {
            this.motor = motor;
            this.autoReset = new AutoResetEvent(false);
        }

        public void StartPattern(SimplePattern pattern)
        {
            lock (syncLock)
            {
                if (t != null)
                {
                    t.Interrupt();
                    t.Join();
                }
                this.pattern = pattern;
                t = new Thread(Run);
                t.Start();
            }
        }

        private void Run()
        {
            for (int i = 0; i < pattern.GetTimings().Length; i++)
            {
                int time = pattern.GetTimings()[i];
                double amplitude = pattern.GetAmplitudes()[i];
                motor.Vibrate(amplitude);
                try
                {
                    Thread.Sleep(time);
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Pattern interrupted: " + pattern.ToString() + " on thread " + this.ToString());
                    break;
                }
            }

            motor.Stop();
        }
    }
}
