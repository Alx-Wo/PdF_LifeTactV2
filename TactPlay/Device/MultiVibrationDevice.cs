using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactPlay.Pattern;

namespace TactPlay.Device
{
    class MultiVibrationDevice : IVibrationDevice
    {
        private List<IVibrationDevice> devices;
        private int numberOfMotors;
        public MultiVibrationDevice(List<IVibrationDevice> devices)
        {
            this.devices = devices;
            if (devices.Count == 0)
            {
                throw new Exception("List is empty");
            }
            this.numberOfMotors = devices.ElementAt(0).GetNumberOfMotors();
            foreach (IVibrationDevice device in devices)
            {
                if (device.GetNumberOfMotors() != numberOfMotors)
                {
                    throw new Exception("Devices have different number of motors");
                }
            }
        }

        public int GetNumberOfMotors()
        {
            return numberOfMotors;
        }

        public void PlayPattern(IPattern pattern)
        {
            foreach (IVibrationDevice device in devices)
            {
                device.PlayPattern(pattern);
            }
        }
    }
}
