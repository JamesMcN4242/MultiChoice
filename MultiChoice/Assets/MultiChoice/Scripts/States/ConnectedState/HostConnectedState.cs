﻿////////////////////////////////////////////////////////////
/////   HostConnectedState.cs
/////   James McNeil - 2021
////////////////////////////////////////////////////////////


using PersonalFramework;
using System.Collections.Generic;
using UnityEngine;

public class HostConnectedState : FlowStateBase
{
    private const float k_timeBeforeReveal = 2.0f;
    private const float k_minTimeBeforeNewHighlight = 0.15f;

    private Dictionary<string, List<string>> m_dataPresets = null;
    private StateController m_childStates = new StateController();
    private ConnectedUI m_connectedUI = null;
    private HostConnection m_networkManager = null;
    private List<string> m_options = null;
    private string[] m_networkCode = null;

    private bool m_selecting = false;
    private float m_timeSelecting = 0.0f;
    private float m_lastHighlightSpot = -1.0f;
    private int m_selectedIndex = 0;

    public HostConnectedState(HostConnection networkManager, List<string> options, string[] code, Dictionary<string, List<string>> dataPresets)
    {
        m_networkManager = networkManager;
        m_options = options;
        m_networkCode = code;
        m_dataPresets = dataPresets;

        m_networkManager.OnNewConnection = () =>
        {
            NetworkPacket packet = new NetworkPacket() { m_messageType = MessageType.CONTENT_OPTIONS, m_content = m_options };
            m_networkManager.SendData(packet, m_networkManager.CurrentClients -1, m_networkManager.CurrentClients);
        };
    }

    protected override void StartPresentingState()
    {
        m_connectedUI.SetSelectionButtonEnabled(true);
        m_connectedUI.BuildGridElements(m_options, 0);
        m_connectedUI.SetConnectionCode(m_networkCode);
    }

    protected override void UpdateActiveState()
    {
        m_childStates.UpdateStack();
        UpdateSelecting();

        object data = m_networkManager.UpdateConnection();
        if(!(data is bool))
        {
            Debug.Log("Received some form of message!");
            List<(int, NetworkPacket)> messages = (List<(int, NetworkPacket)>)data;

            for(int i = 0; i < messages.Count; i++)
            {
                NetworkPacket packet = messages[i].Item2;

                //Relay information to every other connection than the sender
                m_networkManager.SendData(packet, messages[i].Item1);
                
                switch(packet.m_messageType)
                {
                    case MessageType.EDIT_LIST:
                        m_options = (List<string>)packet.m_content;
                        m_connectedUI.BuildGridElements(m_options, 0);
                        break;
                }
            }
        }
    }

    protected override void HandleMessage(object message)
    {
        switch(message)
        {
            case "select":
                m_networkManager.SendData(new NetworkPacket() { m_messageType = MessageType.SELECTION, m_content = null });
                m_selecting = true;
                break;

            case "edit":
                EditActivePresetState editState = new EditActivePresetState(string.Join(", ", m_options), (string editedContent) =>
                {
                    string[] contentArr = editedContent.Replace(", ", ",").Split(',');
                    m_options = new List<string>(contentArr);
                    m_connectedUI.BuildGridElements(m_options, 0);
                    m_networkManager.SendData(new NetworkPacket() { m_messageType = MessageType.EDIT_LIST, m_content = m_options });
                });
                m_childStates.PushState(editState);
                break;

            case "save":
                CreatePresetState createState = new CreatePresetState(m_dataPresets, true, "", string.Join(", ", m_options));
                m_childStates.PushState(createState);
                break;

            case "back":
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
        m_networkManager.OnNewConnection = null;
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

                m_networkManager.SendData(new NetworkPacket() { m_messageType = MessageType.FINAL_SELECTION, m_content = m_selectedIndex });
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
