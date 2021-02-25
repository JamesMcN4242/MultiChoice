////////////////////////////////////////////////////////////
/////   HostConnection.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

using static UnityEngine.Debug;

public class HostConnection : NetworkConnection
{
    private readonly object k_listLock = new object();
    private List<TcpClient> m_clients = new List<TcpClient>(5);
    private List<NetworkStream> m_networkStreams = new List<NetworkStream>(5);

    private int m_currentClients = 0;

    public Action OnNewConnection { set; private get; }

    public HostConnection()
    {
        StartHosting();
    }

    ~HostConnection()
    {
        for(int i = 0; i < m_clients.Count; i++)
        {
            m_networkStreams[i]?.Close();
            m_clients[i]?.Close();
        }
    }

    public override NetworkType GetNetworkType()
    {
        return NetworkType.HOST;
    }

    public override void SendData(object data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        bf.Serialize(memoryStream, data);
        byte[] bytes = memoryStream.ToArray();

        lock (k_listLock)
        {
            for (int i = 0; i < m_networkStreams.Count; i++)
            {
                m_networkStreams[i].Write(bytes, 0, bytes.Length);
            }
        }
    }

    public override object UpdateConnection()
    {
        lock(k_listLock)
        {
            if (m_currentClients < m_clients.Count)
            {
                OnNewConnection?.Invoke();
                m_currentClients = m_clients.Count;
            }

            List<(int bytes, byte[] data)> returnData = new List<(int bytes, byte[] data)>(m_networkStreams.Count);

            for (int i = 0; i < m_networkStreams.Count; i++)
            {
                if (m_networkStreams[i].DataAvailable)
                {
                    byte[] data = new byte[256];
                    int bytes = m_networkStreams[i].Read(data, 0, data.Length);
                    returnData.Add((bytes, data));
                }
            }

            if (returnData.Count > 0)
            {
                return returnData;
            }
        }

        return false;
    }

    private void StartHosting()
    {
        m_backgroundThread = new System.Threading.Thread(StartServer);
        m_backgroundThread.IsBackground = true;
        m_backgroundThread.Start();
    }

    private void StartServer()
    {
        TcpListener server = null;
        try
        {
            server = new TcpListener(IPCodingSystem.GetLocalIPAddress(), k_serverPort);
            server.Start();

            // Enter the listening loop.
            while (true)
            {
                Log("Waiting for a connection... ");
                TcpClient client = server.AcceptTcpClient();

                Log("New client connected!");
                NetworkStream stream = client.GetStream();

                lock(k_listLock)
                {
                    m_clients.Add(client);
                    m_networkStreams.Add(stream);
                }
            }
        }
        catch (SocketException e)
        {
            LogFormat("SocketException: {0}", e);
        }
        finally
        {
            // Stop listening for new clients.
            server.Stop();
        }
    }
}
