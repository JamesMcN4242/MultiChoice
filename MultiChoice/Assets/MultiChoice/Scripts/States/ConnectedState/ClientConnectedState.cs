////////////////////////////////////////////////////////////
/////   ClientConnectedState.cs
/////   James McNeil - 2021
////////////////////////////////////////////////////////////

using PersonalFramework;
using System.Collections.Generic;
using UnityEngine;

public class ClientConnectedState : FlowStateBase
{
    private const float k_timeBeforeReveal = 2.0f;
    private const float k_minTimeBeforeNewHighlight = 0.15f;

    private ConnectedUI m_connectedUI = null;
    private ClientConnection m_networkManager = null;
    private List<string> m_options = null;
    private string[] m_networkCode = null;

    private bool m_selecting = false;
    private float m_timeSelecting = 0.0f;
    private float m_lastHighlightSpot = -1.0f;
    private int m_selectedIndex = 0;

    public ClientConnectedState(ClientConnection networkManager, string[] code)
    {
        m_networkManager = networkManager;
        m_networkCode = code;
    }

    ~ClientConnectedState()
    {
        m_connectedUI.ClearGridElements();
    }

    protected override void StartPresentingState()
    {
        m_connectedUI.SetConnectionCode(m_networkCode);
    }

    protected override void UpdateActiveState()
    {
        UpdateSelecting();
        object data = m_networkManager.UpdateConnection();

        if(!(data is bool))
        {
            Debug.Log("Received some form of message!");

            NetworkPacket packet = (NetworkPacket)data;
            switch (packet.m_messageType)
            {
                case MessageType.CONTENT_OPTIONS:
                    m_options = (List<string>)packet.m_content;
                    m_connectedUI.BuildGridElements(m_options, 0);
                    break;

                case MessageType.SELECTION:
                    //TODO: Decide on the same final target
                    m_selecting = true;
                    break;
            }
        }
    }

    protected override void HandleMessage(object message)
    {
        switch(message)
        {
            case "back":
                m_networkManager.SendData(new NetworkPacket() { m_messageType = MessageType.LEFT_LOBBY, m_content = null });
                ControllingStateStack.PopState(this);
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_connectedUI = Object.FindObjectOfType<ConnectedUI>();
        m_ui = m_connectedUI;
        return m_ui != null;
    }

    protected override void StartDismissingState()
    {
        m_networkManager.ShutDown();
    }

    private void UpdateSelecting()
    {
        if (m_selecting)
        {
            m_timeSelecting += Time.deltaTime;

            if (m_timeSelecting >= k_timeBeforeReveal)
            {
                m_timeSelecting = 0.0f;
                m_selecting = false;
                m_lastHighlightSpot = -1.0f;

                m_connectedUI.SetElementColour(m_selectedIndex, Color.green);
            }
            else if (m_timeSelecting - m_lastHighlightSpot > k_minTimeBeforeNewHighlight + Random.Range(0.0f, 0.3f))
            {
                m_connectedUI.SetElementColour(m_selectedIndex, Color.white);

                int newIndex;
                do
                {
                    newIndex = Random.Range(0, Mathf.Min(m_options.Count, ConnectedUI.k_elementsPerGrid));
                } while (newIndex == m_selectedIndex && m_options.Count > 1);

                m_selectedIndex = newIndex;
                m_connectedUI.SetElementColour(m_selectedIndex, Color.yellow);
                m_lastHighlightSpot = m_timeSelecting;
            }
        }
    }
}
