using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TactPlay.Connection;
using TactPlay.Device;
using TactPlay.Device.Virtual;

namespace TactPlay
{
    public partial class Main : Form
    {
        private AmmunitionEventHandler ammunitionEventHandler = new AmmunitionEventHandler();
        private HealthEventHandler healthEventHandler = new HealthEventHandler();
        private StaminaEventHandler staminaEventHandler = new StaminaEventHandler();
        private VirtualVibrationDevice virtualVibrationDevice = new VirtualVibrationDevice();
        private LogFileReader lfr;
        private Thread lfrThread;
        public static IVibrationDevice vibrationDevice;
        private SerialConnection tactPlayConnection;

        public Main()
        {
            InitializeComponent();
            Setup();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lfrThread = new Thread(lfr.Run);
            lfrThread.Start();
        }

        private void Setup()
        {
            //virtualVibrationDevice.Show();
            UpdateCOMPorts();
            buttonUpdateCOMPorts.Font = new Font("Wingdings 3", 9, FontStyle.Bold);
            buttonUpdateCOMPorts.Text = Char.ConvertFromUtf32(80);
            buttonUpdateCOMPorts.Width = buttonUpdateCOMPorts.Height;
            tactPlayConnection = new SerialConnection(true);
            tactPlayConnection.ConnectToDevice();
            VibrationDevice vibDevice = new VibrationDevice(tactPlayConnection);
            vibrationDevice = new MultiVibrationDevice(new List<IVibrationDevice>() { /*virtualVibrationDevice, */vibDevice });

            LogFileHandler lfh = new LogFileHandler(ammunitionEventHandler, healthEventHandler, staminaEventHandler);
            lfr = new LogFileReader(getLogFilePath(), lfh, true);

            PatternTester patternTester = new PatternTester();
            patternTester.Show();
        }

        //TODO: Remove, we dont need the arma logfile stuff
        private String getLogFilePath()
        {
            //String directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Arma 3\\");
            String directory = @"C:";
            String[] files = Directory.GetFiles(directory);
            if (files == null || files.Length == 0)
            {
                throw new Exception("No file found");
            }
            Array.Sort(files);
            String filePath = "";
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Contains("Arma3") && files[i].EndsWith(".rpt"))
                {
                    filePath = files[i];
                }
            }
            return filePath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lfr.StopReading();
            ammunitionEventHandler.Disable();
            staminaEventHandler.Disable();
            healthEventHandler.Disable();
            //TODO stop everything
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (tactPlayConnection != null)
            {
                tactPlayConnection.ConnectToDevice();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tactPlayConnection != null)
            {
                tactPlayConnection.SendBytes(Connection.Utils.StringToByteArray(textBox1.Text));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MotorTest();
        }

        private async void MotorTest()
        {
            int numVibrations = 1;
            if (tactPlayConnection != null)
            {
                tactPlayConnection.SendBytes(Connection.Utils.StringToByteArray("FF0000000000000000"));
                await Task.Delay(1000);
                for (int motor = 6; motor < 8; motor++)
                {
                    for (int i = 0; i < numVibrations; i++)
                    {
                        tactPlayConnection.SendBytes(Connection.Utils.StringToByteArray("0" + motor + "FF"));
                        await Task.Delay(200);
                        tactPlayConnection.SendBytes(Connection.Utils.StringToByteArray("0" + motor + "00"));
                        if (i != numVibrations - 1)
                        {
                            await Task.Delay(4000);
                        }
                    }
                    await Task.Delay(1000);
                }
            }
        }

        private void buttonMode0_Click(object sender, EventArgs e)
        {
            ammunitionEventHandler.Disable();
            staminaEventHandler.Disable();
            healthEventHandler.Disable();
        }

        private void buttonMode1_Click(object sender, EventArgs e)
        {
            ammunitionEventHandler.Disable();
            staminaEventHandler.Disable();
            healthEventHandler.Disable();
            ammunitionEventHandler.Enable();
        }

        private void buttonMode2_Click(object sender, EventArgs e)
        {
            ammunitionEventHandler.Disable();
            staminaEventHandler.Disable();
            healthEventHandler.Disable();
            staminaEventHandler.Enable();
        }

        private void buttonMode3_Click(object sender, EventArgs e)
        {
            ammunitionEventHandler.Disable();
            staminaEventHandler.Disable();
            healthEventHandler.Disable();
            ammunitionEventHandler.Enable();
            staminaEventHandler.Enable();
        }

        private void UpdateCOMPorts()
        {
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            var items = comboBoxCOMPort.Items;
            foreach (var item in items)
            {
                if (!ports.Contains(item))
                {
                    comboBoxCOMPort.Items.Remove(item);
                }
            }
            foreach (var port in ports)
            {
                if (!comboBoxCOMPort.Items.Contains(port))
                {
                    comboBoxCOMPort.Items.Add(port);
                }
            }
        }

        private void comboBoxCOMPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            tactPlayConnection.ChangePort(comboBoxCOMPort.SelectedItem.ToString());
        }

        private void buttonUpdateCOMPorts_Click(object sender, EventArgs e)
        {
            UpdateCOMPorts();
        }

        int i = 0;
        private void button6_Click(object sender, EventArgs e)
        {
            staminaEventHandler.Enable();
            switch (i)
            {
                case 0:
                    i++;
                    staminaEventHandler.EventTimer(100, 100);
                    break;
                case 1:
                    i++;
                    staminaEventHandler.EventTimer(50, 100);
                    break;
                case 2:
                    i++;
                    staminaEventHandler.EventTimer(0, 100);
                    break;
            }
        }
        public class WindowFinder
        {
            // For Windows Mobile, replace user32.dll with coredll.dll
            [DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

            public static IntPtr FindWindow(String caption)
            {
                return FindWindow(null, caption);
            }

        }
        [DllImport("User32.dll")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess,
        int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        int stop = 0;
        private async void button7_Click(object sender, EventArgs e)
        {
            Int32 Adress = int.Parse(textBox2.Text, System.Globalization.NumberStyles.HexNumber);
            this.stop = 0;
            int oldValue = 100;
            while (this.stop == 0)
            {
                const int PROCESS_WM_READ = 0x0010;
                try
                {
                    Process process = Process.GetProcessesByName("hl2")[0];
                    IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);
                    //Int32 Adress = 0x09AE3720;
                    

                    int bytesRead = 0;
                    byte[] buffer = new byte[1]; //To read a 1 byte entry

                    ReadProcessMemory((int)processHandle, Adress, buffer, buffer.Length, ref bytesRead);

                    
                    if (oldValue != buffer[0])
                    {
                        
                        oldValue = buffer[0];
                        staminaEventHandler.EventTimer(((double)buffer[0]/(double)100), 100);
                        Console.WriteLine(buffer[0]);
                    }
                    //
                    await Task.Delay(100);
                }
                catch (IndexOutOfRangeException ex)
                {
                    MessageBox.Show("Probably hl2 does not run" + ex.StackTrace);
                    this.stop = 1;
                }
                

                
            }
            
            //this.textBox2.Text =FindWindow(null, "Powerstation 17").ToString();
            //IntPtr hwnd = WindowFinder.FindWindow("Powerstation 17");
            //uint pid;
            //this.textBox2.Text = WindowFinder.FindWindow("Powerstation 17").ToString();
            //GetWindowThreadProcessId(hwnd, pid);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.stop = 1;
        }

    }
}
