using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactPlay.Connection
{
    interface ITactPlayConnection
    {
        void ConnectToDevice();
        void CloseConnection();
        void SendBytes(byte[] bytes);
    }
}
