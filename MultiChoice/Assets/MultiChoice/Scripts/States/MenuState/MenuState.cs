////////////////////////////////////////////////////////////
/////   MenuState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

public class MenuState : FlowStateBase
{
    protected override bool AquireUIFromScene()
    {
        m_ui = Object.FindObjectOfType<MenuUI>();
        return m_ui != null;
    }

    protected override void HandleMessage(object message)
    {
        switch(message)
        {
            case "host":
                ControllingStateStack.PushState(new HostState());
                break;

            case "join":
                ControllingStateStack.PushState(new JoinState());
                break;

            case "exit":
                Application.Quit();
                break;
        }
    }
}
