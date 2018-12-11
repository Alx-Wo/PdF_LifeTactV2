using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace TactPlay.Connection
{
    class SerialConnection : ITactPlayConnection
    {
        private SerialPort serialPort;
        private String portName;
        private SendVisualizer visualizer;
        public SerialConnection(String portName, bool useVisualizer)
        {
            if (useVisualizer)
            {
                visualizer = new SendVisualizer();
                visualizer.Show();
            }
            this.portName = portName;
        }

        public SerialConnection(bool useVisualizer)
            : this(null, useVisualizer)
        {
        }

        public void ChangePort(String portName)
        {
            bool connectionWasOpen = false;
            if (this.portName != null)
            {
                connectionWasOpen = serialPort.IsOpen;
                CloseConnection();
            }
            this.portName = portName;
            if (connectionWasOpen)
            {
                ConnectToDevice();
            }
        }

        public void CloseConnection()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        public void ConnectToDevice()
        {
            if (portName == null)
            {
                return;
            }
            if (serialPort == null || !serialPort.IsOpen)
            {
                serialPort = new SerialPort(portName, 9600);
                serialPort.Open();
            }
        }

        public void SendBytes(byte[] bytes)
        {
            if (visualizer != null)
            {
                visualizer.AddValue(bytes);
            }
            if (serialPort == null || !serialPort.IsOpen)
            {
                return;
            }
            byte[] buffer = new byte[bytes.Length];
            Array.Copy(bytes, buffer, bytes.Length);
            serialPort.Write(buffer, 0, buffer.Length);
        }
    }
}
