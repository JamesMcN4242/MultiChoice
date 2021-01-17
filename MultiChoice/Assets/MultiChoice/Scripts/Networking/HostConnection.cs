////////////////////////////////////////////////////////////
/////   HostConnection.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using static UnityEngine.Debug;

public class HostConnection : NetworkConnection
{    
    public HostConnection()
    {
        StartHosting();
    }

    public override NetworkType GetNetworkType()
    {
        return NetworkType.HOST;
    }

    private void StartHosting()
    {
        string[] code =  IPCodingSystem.CalculateConnectionCode();
        IPAddress ipAddress = IPAddress.Parse(IPCodingSystem.GetIPFromCode(code));
        //StartServer(ipAddress);
    }

    private static void StartServer(IPAddress hostAddress)
    {
        IPEndPoint localEndPoint = new IPEndPoint(hostAddress, 11000);

        try
        {
            // Create a Socket that will use Tcp protocol      
            Socket listener = new Socket(hostAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // A Socket must be associated with an endpoint using the Bind method  
            listener.Bind(localEndPoint);
            // Specify how many requests a Socket can listen before it gives Server busy response.  
            // We will listen 10 requests at a time  
            listener.Listen(10);

            Log("Waiting for a connection...");
            Socket handler = listener.Accept();

            // Incoming data from the client.    
            string data = null;
            byte[] bytes = null;

            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }

            Log("Text received : " + data);

            byte[] msg = Encoding.ASCII.GetBytes(data);
            handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch (Exception e)
        {
            Log(e.ToString());
        }

        Log("\n Press any key to continue...");
        Console.ReadKey();
    }
}
