////////////////////////////////////////////////////////////
/////   NetworkManager.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

public class NetworkManager
{
    private NetworkConnection m_networkController = null;

    public NetworkManager(bool isHost)
    {
        if(isHost)
        {
            m_networkController = new HostConnection();
        }
        else
        {
            m_networkController = new ClientConnection();
        }
    }
}
