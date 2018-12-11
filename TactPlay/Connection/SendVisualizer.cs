using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace TactPlay.Connection
{
    public partial class SendVisualizer : Form
    {
        private KeyValuePair<long, byte[]>[] valueArray = new KeyValuePair<long, byte[]>[50];
        private static readonly int TIME_TO_SHOW = 3000;
        private static readonly int NUMBER_OF_MOTORS = 8;

        public SendVisualizer()
        {
            InitializeComponent();

            for (int i = 0; i < valueArray.Length; i++)
            {
                valueArray[i] = new KeyValuePair<long, byte[]>(Environment.TickCount, new byte[] { 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            }

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(UpdateBitmap);
            timer.Interval = 20;
            timer.Start();
        }

        private void UpdateBitmap(object sender, ElapsedEventArgs e)
        {
            PaintValues();
        }

        private void ShiftValueArray()
        {
            for (int i = 1; i < valueArray.Length; i++)
            {
                valueArray[i - 1] = valueArray[i];
            }
        }

        public void AddValue(byte[] bytes)
        {
            if (bytes.Length != NUMBER_OF_MOTORS + 1)
            {
                return;
            }
            byte[] copy = new byte[bytes.Length];
            Array.Copy(bytes, copy, bytes.Length);
            ShiftValueArray();
            valueArray[valueArray.Length - 1] = new KeyValuePair<long, byte[]>(Environment.TickCount, copy);
        }

        private double GetDoubleForByte(byte[] bytes, int index)
        {
            return bytes[index] / 255.0;
        }

        private void PaintValues()
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bitmap);
            long time = Environment.TickCount;
            Pen p = new Pen(Brushes.DarkRed, 1);
            g.Clear(Color.White);

            int spaceBetweenGraphs = 3;
            for (int i = 1; i < valueArray.Length; i++)
            {
                KeyValuePair<long, byte[]> value1 = valueArray[i - 1];
                KeyValuePair<long, byte[]> value2 = valueArray[i];

                for (int motor = 0; motor < NUMBER_OF_MOTORS; motor++)
                {
                    int width = bitmap.Width;
                    int height = (bitmap.Height / NUMBER_OF_MOTORS) - spaceBetweenGraphs;
                    int heightOffset = (height + spaceBetweenGraphs) * motor;

                    if (i == 1)
                    {
                        //Draw space between graphs
                        g.FillRectangle(Brushes.LightGray, 0, heightOffset - spaceBetweenGraphs, width, spaceBetweenGraphs);
                    }

                    int x1 = GetXCoordinate(width, time, value1.Key);
                    int y1 = GetYCoordinate(height, time, GetDoubleForByte(value1.Value, motor + 1), heightOffset);

                    int x2 = GetXCoordinate(width, time, value2.Key);
                    int y2 = GetYCoordinate(height, time, GetDoubleForByte(value2.Value, motor + 1), heightOffset);

                    g.DrawLine(p, x1, y1, x2, y1);
                    g.DrawLine(p, x2, y1, x2, y2);
                    if (i == valueArray.Length - 1)
                    {
                        //Draw line from last data point to the right side of the screen
                        g.DrawLine(p, x2, y2, width, y2);
                    }
                }
            }
            pictureBox1.BackgroundImage = bitmap;
        }

        private int GetXCoordinate(int width, long time, long value)
        {
            return (int)Math.Round(width - ((double)width / TIME_TO_SHOW) * (time - value));
        }

        private int GetYCoordinate(int height, long time, double value, int heightOffset)
        {
            return (int)Math.Round(height - ((double)height * value)) + heightOffset;
        }
    }
}
