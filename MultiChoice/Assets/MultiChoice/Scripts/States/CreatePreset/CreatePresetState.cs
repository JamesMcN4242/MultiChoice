////////////////////////////////////////////////////////////
/////   CreatePresetState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System.Collections.Generic;
using UnityEngine;

public class CreatePresetState : FlowStateBase
{
    private readonly bool k_shouldBeValidated;

    private Dictionary<string, List<string>> m_presets;
    private CreatePresetUI m_createUI = null;

    private bool m_validated = true;

    public CreatePresetState(Dictionary<string, List<string>> presets, bool shouldBeValidated = true)
    {
        m_presets = presets;
        k_shouldBeValidated = shouldBeValidated;
    }

    protected override void StartPresentingState()
    {
        if (k_shouldBeValidated)
        {
            m_createUI.AddPresetListener(ValidatePresetName);
        }
    }

    protected override void UpdateActiveState()
    {
        string name = m_createUI.GetPresetName();
        string content = m_createUI.GetPresetContent();
        bool canBeCreated = !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(content);

        m_createUI.SetCreateButtonEnabled(canBeCreated && m_validated);
    }

    protected override bool AquireUIFromScene()
    {
        m_createUI = Object.FindObjectOfType<CreatePresetUI>();
        m_ui = m_createUI;
        return m_ui != null;
    }

    protected override void HandleMessage(object message)
    {
        switch(message)
        {
            case "createPreset":
                string name = m_createUI.GetPresetName();
                string[] contentArr = m_createUI.GetPresetContent().Replace(", ", ",").Split(',');
                List<string> content = new List<string>(contentArr);
                m_presets[name] = content;
                PresetDataSystem.SaveNewPresets(m_presets);

                ControllingStateStack.PopState(this);
                break;
        }
    }

    protected override void StartDismissingState()
    {
        m_createUI.ClearPresetListeners();
    }

    private void ValidatePresetName(string input)
    {
        m_validated = !m_presets.ContainsKey(input);
        m_createUI.SetNameValidated(m_validated);
    }
}
