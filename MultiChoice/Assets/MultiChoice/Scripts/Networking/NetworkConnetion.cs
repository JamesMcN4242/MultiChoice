////////////////////////////////////////////////////////////
/////   NetworkConnection.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

public enum NetworkType
{
    HOST,
    CLIENT
};

public abstract class NetworkConnection
{
    public abstract NetworkType GetNetworkType();

    public bool IsHost()
    {
        return GetNetworkType() == NetworkType.HOST;
    }

    public bool IsConnection()
    {
        return GetNetworkType() == NetworkType.CLIENT;
    }
}
