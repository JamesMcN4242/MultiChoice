////////////////////////////////////////////////////////////
/////   ClientConnection.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

public class ClientConnection : NetworkConnection
{    
    public override NetworkType GetNetworkType()
    {
        return NetworkType.CLIENT;
    }
}
