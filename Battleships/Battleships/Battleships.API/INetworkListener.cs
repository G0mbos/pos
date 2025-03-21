namespace Battleships.API
{
    public interface INetworkListener<T>
    {
        public void OnDisconnected(Transfer<T> t);
        public void OnMSG(T msg, Transfer<T> t);
        public void OnDebug(string msg, Transfer<T> t);
    }
}