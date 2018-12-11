using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactPlay.Pattern;

namespace TactPlay.Device
{
    public interface IVibrationDevice
    {
        void PlayPattern(IPattern pattern);

        int GetNumberOfMotors();
    }
}
