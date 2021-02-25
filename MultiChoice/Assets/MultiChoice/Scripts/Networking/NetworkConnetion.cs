////////////////////////////////////////////////////////////
/////   NetworkConnection.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum NetworkType
{
    HOST,
    CLIENT
};

public abstract class NetworkConnection
{
    protected const int k_serverPort = 15032;

    private BinaryFormatter m_binaryFormatter = new BinaryFormatter();
    protected byte[] m_receivedBuffer = new byte[2048];

    public abstract NetworkType GetNetworkType();
    public abstract object UpdateConnection();
    public abstract void SendData(object data);
    public abstract void ShutDown();

    public bool IsHost()
    {
        return GetNetworkType() == NetworkType.HOST;
    }

    public bool IsConnection()
    {
        return GetNetworkType() == NetworkType.CLIENT;
    }

    protected byte[] ConvertToByteArray(object data)
    {
        MemoryStream memoryStream = new MemoryStream();
        m_binaryFormatter.Serialize(memoryStream, data);
        return memoryStream.ToArray();
    }

    protected NetworkPacket ConvertReceivedToNetworkPacket(int bytes)
    {
        MemoryStream memStream = new MemoryStream();
        memStream.Write(m_receivedBuffer, 0, bytes);
        memStream.Seek(0, SeekOrigin.Begin);
        return (NetworkPacket)m_binaryFormatter.Deserialize(memStream);
    }
}
