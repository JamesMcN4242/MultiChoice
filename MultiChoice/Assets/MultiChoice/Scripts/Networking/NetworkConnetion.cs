////////////////////////////////////////////////////////////
/////   NetworkConnection.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System.Threading;

public enum NetworkType
{
    HOST,
    CLIENT
};

public abstract class NetworkConnection
{
    protected const int k_serverPort = 15032;

    protected Thread m_backgroundThread = null;

    ~NetworkConnection()
    {
        if(m_backgroundThread != null)
        {
            m_backgroundThread.Join(10);
        }
    }

    public abstract NetworkType GetNetworkType();
    public abstract object UpdateConnection();
    public abstract void SendData(object data);

    public bool IsHost()
    {
        return GetNetworkType() == NetworkType.HOST;
    }

    public bool IsConnection()
    {
        return GetNetworkType() == NetworkType.CLIENT;
    }
}
