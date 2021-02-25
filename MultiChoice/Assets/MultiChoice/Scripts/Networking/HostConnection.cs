////////////////////////////////////////////////////////////
/////   HostConnection.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

using static UnityEngine.Debug;

public class HostConnection : NetworkConnection
{
    private readonly object k_listLock = new object();
    private List<TcpClient> m_clients = new List<TcpClient>(5);
    private List<NetworkStream> m_networkStreams = new List<NetworkStream>(5);
    private TcpListener m_tcpServer = null;
    private Thread m_backgroundThread = null;

    private int m_currentClients = 0;

    public Action OnNewConnection { set; private get; }

    public HostConnection()
    {
        StartHosting();
    }

    public override NetworkType GetNetworkType()
    {
        return NetworkType.HOST;
    }

    public override void SendData(object data)
    {
        byte[] bytes = ConvertToByteArray(data);

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

            List<NetworkPacket> returnData = new List<NetworkPacket>(m_networkStreams.Count);

            for (int i = 0; i < m_networkStreams.Count; i++)
            {
                if (m_networkStreams[i].DataAvailable)
                {
                    int bytes = m_networkStreams[i].Read(m_receivedBuffer, 0, m_receivedBuffer.Length);
                    returnData.Add(ConvertReceivedToNetworkPacket(bytes));
                }
            }

            if (returnData.Count > 0)
            {
                return returnData;
            }
        }

        return false;
    }

    public override void ShutDown()
    {
        for (int i = 0; i < m_clients.Count; i++)
        {
            m_networkStreams[i]?.Close();
            m_clients[i]?.Close();
        }

        if (m_backgroundThread != null)
        {
            m_backgroundThread.Join(0);
        }

        m_tcpServer?.Stop();
    }

    private void StartHosting()
    {
        m_backgroundThread = new System.Threading.Thread(RunServer);
        m_backgroundThread.IsBackground = true;
        m_backgroundThread.Start();
    }

    private void RunServer()
    {
        try
        {
            m_tcpServer = new TcpListener(IPCodingSystem.GetLocalIPAddress(), k_serverPort);
            m_tcpServer.Start();

            // Enter the listening loop.
            while (true)
            {
                Log("Waiting for a connection... ");
                TcpClient client = m_tcpServer.AcceptTcpClient();

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
            m_tcpServer?.Stop();
        }
    }
}