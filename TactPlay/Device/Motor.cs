using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactPlay.Device
{
    public interface IMotor
    {
        void Vibrate(long millis, double amplitude);
        void Vibrate(double amplitude);
        void Stop();
    }
}
