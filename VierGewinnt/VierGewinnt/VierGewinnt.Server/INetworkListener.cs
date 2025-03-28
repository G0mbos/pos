﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VierGewinnt.Server;

namespace VierGewinnt.Server
{
    public interface INetworkListener<T>
    {
        public void OnDisconnected(Transfer<T> t);
        public void OnMSG(T msg, Transfer<T> t);
        public void OnDebug(string msg, Transfer<T> t);
    }
}
