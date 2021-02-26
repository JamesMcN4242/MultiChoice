////////////////////////////////////////////////////////////
/////   GenericPopupState.cs
/////   James McNeil - 2021
////////////////////////////////////////////////////////////

using PersonalFramework;
using UnityEngine;

public class GenericPopupState : FlowStateBase
{
    private readonly string k_contentText = null;
    private readonly string k_buttonText = null;

    private GenericPopupUI m_popupUI = null;

    public GenericPopupState(string messageText, string buttonText = "Okay")
    {
        k_contentText = messageText;
        k_buttonText = buttonText;
    }

    protected override void StartPresentingState()
    {
        m_popupUI.SetPopupText(k_contentText);
        m_popupUI.SetButtonText(k_buttonText);
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case "dismiss":
                ControllingStateStack.PopState(this);
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_popupUI = Object.FindObjectOfType<GenericPopupUI>();
        m_ui = m_popupUI;
        return m_ui != null;
    }
}
