using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Net;

public interface INetworkListener<T>
{
    public void OnDisconnected(Transfer<T> t);
    public void OnMSG(T msg, Transfer<T> t);
    public void OnDebug(string msg, Transfer<T> t);
}