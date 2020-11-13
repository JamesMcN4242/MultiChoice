﻿////////////////////////////////////////////////////////////
/////   HostState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System.Collections.Generic;
using UnityEngine;

public class HostState : FlowStateBase
{
    private HostUI m_hostUI = null;
    private Dictionary<string, List<string>> m_dataPresets = null;
    private string m_selectedPresetKey = null;

    protected override void StartPresentingState()
    {
        m_dataPresets = PresetDataSystem.LoadAllPresets();
        RefreshUI();
    }

    public override void ReenteredForeground()
    {
        RefreshUI();
    }

    protected override bool AquireUIFromScene()
    {
        m_hostUI = Object.FindObjectOfType<HostUI>();
        m_ui = m_hostUI;
        return m_ui != null;
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case "createPreset":
                CreatePresetState createState = new CreatePresetState(m_dataPresets);
                ControllingStateStack.PushState(createState);
                break;

            case "selectPreset":
                SelectPresetState selectState = new SelectPresetState(m_dataPresets);
                ControllingStateStack.PushState(selectState);
                break;

            case "start":
                break;
        }
    }

    public void RefreshUI()
    {
        m_hostUI.SetSelectedPreset(m_selectedPresetKey);
        m_hostUI.SetPresetSelectionEnabled(m_dataPresets.Count > 0);
        m_hostUI.SetStartOptionEnabled(!string.IsNullOrEmpty(m_selectedPresetKey));
    } 
}