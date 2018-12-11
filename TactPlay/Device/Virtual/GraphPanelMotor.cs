using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace TactPlay.Device.Virtual
{
    class GraphPanelMotor : Panel, IMotor
    {
        private System.Timers.Timer timer;
        private Thread motorThread;
        private double currentValue = 0;
        private double[] values = new double[1000];
        private PictureBox pictureBox;

        public GraphPanelMotor()
            : base()
        {
            pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            this.Controls.Add(pictureBox);

            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 10;
            timer.Start();

            System.Timers.Timer paintTimer = new System.Timers.Timer();
            paintTimer.Elapsed += new ElapsedEventHandler(PaintUpdateEvent);
            paintTimer.Interval = 20;
            paintTimer.Start();
        }

        private void PaintUpdateEvent(object source, ElapsedEventArgs e)
        {
            PaintValues();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            double[] tmp = new double[values.Length];
            for (int i = 1; i < values.Length; i++)
            {
                tmp[i - 1] = values[i];
            }
            values = tmp;
            values[values.Length - 1] = currentValue;
        }

        public void SetCurrentValue(double currentValue)
        {
            this.currentValue = currentValue;
        }


        private void PaintValues()
        {
            Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White); //Clear the area

            Pen myPen = new Pen(Color.Black);

            g.DrawLine(myPen, 0, ClientSize.Height - 1, ClientSize.Width, ClientSize.Height - 1); //Draw bottom line


            for (int x = 0; x < ClientSize.Width; x++)
            {
                int iStart = Math.Min((int)Math.Round(((double)values.Length / ClientSize.Width) * x), values.Length);
                int iEnd = Math.Min(Math.Max(iStart, (int)Math.Round(((double)values.Length / ClientSize.Width) * (x + 1)) - 1), values.Length);
                double value = 0; // = maximum
                for (int i = iStart; i <= iEnd; i++)
                {
                    value = Math.Max(value, values[i]);
                    if (value != 0)
                    {
                    }
                }
                int yStart = (int)Math.Round((ClientSize.Height * (1.0 - value)));
                g.DrawLine(myPen, x, yStart, x, ClientSize.Height);
            }
            pictureBox.BackgroundImage = bitmap;
        }

        public void Vibrate(double amplitude)
        {
            SetCurrentValue(amplitude);
        }

        public void Vibrate(long millis, double amplitude)
        {
            if (motorThread != null)
            {
                motorThread.Interrupt();
                try
                {
                    motorThread.Join();
                }
                catch (ThreadInterruptedException e)
                {
                    Console.Error.WriteLine(e.ToString());
                }
            }
            motorThread = new Thread(() => ThreadRun(millis, amplitude));
            motorThread.Start();
        }

        private void ThreadRun(long millis, double amplitude)
        {
            SetCurrentValue(amplitude);
            try
            {
                Thread.Sleep((int)millis);
            }
            catch (ThreadInterruptedException)
            {
                SetCurrentValue(0);
            }
            SetCurrentValue(0);
        }

        public void Stop()
        {
            if (motorThread != null)
            {
                motorThread.Interrupt();
            }
            SetCurrentValue(0);
        }

    }
}
