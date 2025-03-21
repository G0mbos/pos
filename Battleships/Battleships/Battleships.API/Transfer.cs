using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
 
namespace Battleships.API
{
    public class Transfer<T>
    {
        private TcpClient _client;
        private INetworkListener<T> _listener;
        private StreamReader _reader;
        private StreamWriter _writer;
        private XmlSerializer _xml = new XmlSerializer(typeof(T));
        public Transfer(TcpClient client, INetworkListener<T> listener)
        {
            _listener = listener;
            _client = client;
            _reader = new StreamReader(_client.GetStream());
            _writer = new StreamWriter(_client.GetStream());

            ThreadPool.QueueUserWorkItem(y =>
            {
                try
                {
                    String? line;
                    String msg = "";
                    while ((line = _reader.ReadLine()) != null)
                    {
                        msg += line + "\n";
                        //Console.WriteLine(typeof(T).Name);
                        if (line == "</" + typeof(T).Name + ">")
                        {
                            _listener.OnDebug(msg, this);
                            StringReader stringReader = new StringReader(msg);
                            T? ms = (T?)_xml.Deserialize(stringReader);
                            if (ms == null)
                            {
                                _listener.OnDebug(typeof(T).Name + " is null", this);
                            }
                            else
                                _listener.OnMSG(ms, this);
                            msg = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    _listener.OnDebug("Exception: " + ex.Message, this);
                }
                finally
                {
                    _listener.OnDisconnected(this);
                    _reader.Close();
                    _writer.Close();
                    _client.Close();
                }
            });
        }

        public void Close()
        {
            _reader.Close();
            _writer.Close();
            _client.Close();
        }

        public void Send(T msg)
        {
            StringWriter stringWriter = new StringWriter();
            _xml.Serialize(stringWriter, msg);
            _listener.OnDebug(stringWriter.ToString(), this);
            _writer.WriteLine(stringWriter.ToString());
            _writer.Flush();
        }
    }
}