using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactPlay.Connection;

namespace TactPlay.Device
{
    class BLEMotor : IMotor
    {
        private ITactPlayConnection tactPlayConnection;
        private byte motorID;

        public BLEMotor(ITactPlayConnection connection, int motorID)
        {
            this.tactPlayConnection = connection;
            this.motorID = Convert.ToByte(motorID);
        }
        public void Stop()
        {
            byte[] bytes = new byte[] { motorID, Convert.ToByte(0) };
            tactPlayConnection.SendBytes(bytes);
            tactPlayConnection.SendBytes(bytes);
        }

        public void Vibrate(long millis, double amplitude)
        {
            Vibrate(amplitude);
        }

        public void Vibrate(double amplitude)
        {
            byte[] bytes = new byte[] { motorID, Convert.ToByte(Math.Floor(amplitude * 155) + 100) };
            tactPlayConnection.SendBytes(bytes);
        }
    }
}
