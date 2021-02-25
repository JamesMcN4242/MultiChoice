////////////////////////////////////////////////////////////
/////   JoinState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System.Collections.Generic;
using PersonalFramework;
using UnityEngine;

public class JoinState : FlowStateBase
{
    private JoinUI m_joinUI = null;

    protected override void UpdateActiveState()
    {
        bool allFieldsFilled = m_joinUI.DoAllFieldsContainText();
        m_joinUI.SetStartButtonInteractive(allFieldsFilled);
    }

    protected override bool AquireUIFromScene()
    {
        m_joinUI = GameObject.FindObjectOfType<JoinUI>();
        m_ui = m_joinUI;
        return m_ui != null;
    }

    protected override void HandleMessage(object message)
    {
        switch(message)
        {
            case "back":
                ControllingStateStack.PopState(this);
                break;

            case "join":
                string[] connectionCode = m_joinUI.GetCodeInput();
                string ip = IPCodingSystem.GetIPFromCode(connectionCode);
                NetworkManager networkManager = new NetworkManager(ip);

                ClientConnectedState connectedState = new ClientConnectedState(networkManager, connectionCode);
                ControllingStateStack.PushState(connectedState);
                break;
        }
    }

    protected override void StartDismissingState()
    {
        m_joinUI.ClearAllInput();
    }

    private void TryConnectToIP()
    {
        string[] connections = m_joinUI.GetCodeInput();
        string ip = IPCodingSystem.GetIPFromCode(connections);
    }
}
