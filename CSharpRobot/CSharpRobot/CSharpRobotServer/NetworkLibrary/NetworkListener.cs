using NetworkLibrary;
namespace NetworkLibrary
{
    public interface NetworkListener<T>
    {
        void OnDisconnected(Transfer<T> t);
        void OnMSG(T msg, Transfer<T> t);
        void OnDebug(string msg, Transfer<T> t);
    }
}
