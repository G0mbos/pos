using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaschenrechnerNW.API
{
    public interface INetworkListener<T>
    {
        void OnMSG(T msg, Transfer<T> transfer);
        void OnDebug(String msg, Transfer<T> transfer);
        void OnDisconnected(Transfer<T> transfer);
    }
}
