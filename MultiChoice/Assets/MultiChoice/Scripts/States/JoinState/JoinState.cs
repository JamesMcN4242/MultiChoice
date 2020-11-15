////////////////////////////////////////////////////////////
/////   JoinState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

public class JoinState : FlowStateBase
{
    private JoinUI m_joinUI = null;

    protected override void UpdateActiveState()
    {
        bool allFieldsFilled = m_joinUI.DoAllFieldContainText();
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

            case "start":
                string[] connections = m_joinUI.GetCodeInput();
                string ip = IPCodingSystem.GetIPFromCode(connections);
                Debug.Log(ip);
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
