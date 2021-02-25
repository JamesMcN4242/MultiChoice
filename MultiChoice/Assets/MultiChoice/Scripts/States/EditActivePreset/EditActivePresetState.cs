////////////////////////////////////////////////////////////
/////   EditActivePresetState.cs
/////   James McNeil - 2021
////////////////////////////////////////////////////////////

using PersonalFramework;
using System;
using UnityEngine;

public class EditActivePresetState : FlowStateBase
{
    private readonly string k_startingContent;
    private EditActivePresetUI m_createUI = null;
    private Action<string> m_onEditComplete = null;

    public EditActivePresetState(string startContent, Action<string> onFinishedEditing)
    {
        k_startingContent = startContent;
        m_onEditComplete = onFinishedEditing;
    }

    protected override void StartPresentingState()
    {
        m_createUI.SetContent(k_startingContent);
    }

    protected override void UpdateActiveState()
    {
        string content = m_createUI.GetPresetContent();
        m_createUI.SetEditButtonEnabled(!content.Equals(k_startingContent));
    }

    protected override bool AquireUIFromScene()
    {
        m_createUI = GameObject.FindObjectOfType<EditActivePresetUI>();
        m_ui = m_createUI;
        return m_ui != null;
    }

    protected override void StartDismissingState()
    {
        m_onEditComplete = null;
    }

    protected override void HandleMessage(object message)
    {
        switch(message)
        {
            case "editPreset":
                m_onEditComplete?.Invoke(m_createUI.GetPresetContent());
                ControllingStateStack.PopState(this);
                break;

            case "back":
                ControllingStateStack.PopState(this);
                break;
        }
    }
}
