////////////////////////////////////////////////////////////
/////   NetworkPacket.cs
/////   James McNeil - 2021
////////////////////////////////////////////////////////////

[System.Serializable]
public enum MessageType : byte
{
    CONTENT_OPTIONS,
    SELECTION,
    FINAL_SELECTION,
    LEFT_LOBBY
}

[System.Serializable]
public struct NetworkPacket
{
    public MessageType m_messageType;
    public object m_content;
}
