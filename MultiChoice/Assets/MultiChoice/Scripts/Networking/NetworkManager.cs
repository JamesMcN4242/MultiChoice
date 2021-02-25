////////////////////////////////////////////////////////////
/////   NetworkManager.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

public class NetworkManager
{
    private NetworkConnection m_networkController = null;

    public HostConnection GetHostConnection => m_networkController as HostConnection;

    public NetworkManager(string serverIP = null)
    {
        if(serverIP == null)
        {
            m_networkController = new HostConnection();
        }
        else
        {
            m_networkController = new ClientConnection(serverIP);
        }
    }

    public void SendMessage(object msg)
    {
        m_networkController.SendData(msg);
    }

    public object UpdateNetwork()
    {
        return m_networkController.UpdateConnection();
    }
}
