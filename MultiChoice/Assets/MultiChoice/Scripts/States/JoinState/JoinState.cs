﻿////////////////////////////////////////////////////////////
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
        m_joinUI.UpdateTextboxSelection();

        bool anyTextPresent = allFieldsFilled || m_joinUI.AnyFieldContainsText();
        m_joinUI.SetClearButtonInteractive(anyTextPresent);
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
                ClientConnection networkManager = new ClientConnection(ip);

                if (networkManager.ConnectedSuccessfully)
                {
                    ClientConnectedState connectedState = new ClientConnectedState(networkManager, connectionCode);
                    ControllingStateStack.PushState(connectedState);
                }
                else
                {
                    GenericPopupState popup = new GenericPopupState("Failed to connect to the specified lobby");
                    ControllingStateStack.PushState(popup);
                }
                break;

            case "clear":
                m_joinUI.ClearAllInput();
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
