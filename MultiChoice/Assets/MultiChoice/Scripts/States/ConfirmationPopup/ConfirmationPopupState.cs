////////////////////////////////////////////////////////////
/////   ConfirmationPopupState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////
///
/// NOTE: This class is taken directly from another of my project's that can be found here
/// https://github.com/JamesMcN4242/BudgetingApp/blob/master/Assets/BudgetApp/Scripts/States/ConfirmationPopup/ConfirmationPopupState.cs

using PersonalFramework;
using System;
using UnityEngine;

public class ConfirmationPopupState : FlowStateBase
{
    public struct PopupText
    {
        public string m_title;
        public string m_description;
        public string m_accept;
        public string m_decline;
    }

    private const string k_acceptMsg = "accept";
    private const string k_declineMsg = "decline";

    private UIConfirmationPopup m_uiPopup = null;
    private Action m_acceptAction = null;
    private Action m_declineAction = null;
    private PopupText m_popupText;

    public ConfirmationPopupState(PopupText popupText, Action acceptAction, Action declineAction = null)
    {
        m_popupText = popupText;
        m_acceptAction = acceptAction;
        m_declineAction = declineAction;
    }

    protected override void StartPresentingState()
    {
        m_uiPopup.SetText(m_popupText);
    }

    protected override void HandleMessage(object message)
    {
        switch(message)
        {
            case k_acceptMsg:
                m_acceptAction?.Invoke();
                ControllingStateStack.PopState(this);
                break;

            case k_declineMsg:
                m_declineAction?.Invoke();
                ControllingStateStack.PopState(this);
                break;
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_uiPopup = GameObject.FindObjectOfType<UIConfirmationPopup>();
        m_ui = m_uiPopup;
        return m_ui != null;
    }
}
