////////////////////////////////////////////////////////////
/////   ClientConnection.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System;
using System.Net.Sockets;

using static UnityEngine.Debug;

public class ClientConnection : NetworkConnection
{
    private TcpClient m_client = null;
    private NetworkStream m_networkStream = null;

    public bool ConnectedSuccessfully => m_client != null && m_client.Connected;

    public ClientConnection(string ip)
    {
        Connect(ip);
    }

    public override NetworkType GetNetworkType()
    {
        return NetworkType.CLIENT;
    }

    public override object UpdateConnection()
    {
        if(m_client != null && m_client.Connected)
        {
            if(m_networkStream.DataAvailable)
            {
                int bytes = m_networkStream.Read(m_receivedBuffer, 0, m_receivedBuffer.Length);
                return ConvertReceivedToNetworkPacket(bytes);                
            }
        }

        return false;
    }

    public override void SendData(object data)
    {
        byte[] bytes = ConvertToByteArray(data);
        m_networkStream.Write(bytes, 0, bytes.Length);
    }

    public override void ShutDown()
    {
        m_networkStream?.Close();
        m_client?.Close();
    }

    private void Connect(string server)    
    {
        try
        {
            m_client = new TcpClient(server, k_serverPort);
            m_networkStream = m_client.GetStream();
        }
        catch (ArgumentNullException e)
        {
            LogWarningFormat("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            LogWarningFormat("SocketException: {0}", e);
        }
    }
}
