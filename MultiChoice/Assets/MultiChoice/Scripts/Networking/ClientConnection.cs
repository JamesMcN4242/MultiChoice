﻿////////////////////////////////////////////////////////////
/////   ClientConnection.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

using static UnityEngine.Debug;

public class ClientConnection : NetworkConnection
{
    private TcpClient m_client = null;
    private NetworkStream m_networkStream = null;

    public ClientConnection(string ip)
    {
        Connect(ip);
    }

    ~ClientConnection()
    {
        m_networkStream?.Close();
        m_client?.Close();
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
                byte[] data = new Byte[256];
                int bytes = m_networkStream.Read(data, 0, data.Length);
                return (bytes, data);
            }
        }

        return false;
    }

    public override void SendData(object data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        bf.Serialize(memoryStream, data);
        byte[] bytes = memoryStream.ToArray();

        m_networkStream.Write(bytes, 0, bytes.Length);
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
