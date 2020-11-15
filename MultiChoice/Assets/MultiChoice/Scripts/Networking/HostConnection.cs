////////////////////////////////////////////////////////////
/////   HostConnection.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////


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

        //TODO: Remove this debug output
        string comboCode = string.Join(" - ", code);
        UnityEngine.Debug.Log(comboCode);
    }
}
