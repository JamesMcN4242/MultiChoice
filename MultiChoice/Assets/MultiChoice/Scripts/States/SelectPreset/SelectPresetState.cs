////////////////////////////////////////////////////////////
/////   SelectPresetState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System.Collections.Generic;
using UnityEngine;

public class SelectPresetState : FlowStateBase
{
    private SelectPresetUI m_selectPresetUI = null;
    private Dictionary<string, List<string>> m_presets;
    private List<string> m_keys = null;

    public SelectPresetState(Dictionary<string, List<string>> presets)
    {
        m_presets = presets;

        m_keys = new List<string>(m_presets.Count);
        foreach(string key in m_presets.Keys)
        {
            m_keys.Add(key);
        }
    }

    protected override void StartPresentingState()
    {
        m_selectPresetUI.BuildGridElements(m_keys, 0);
    }

    protected override bool AquireUIFromScene()
    {
        m_selectPresetUI = Object.FindObjectOfType<SelectPresetUI>();
        m_ui = m_selectPresetUI;
        return m_ui != null;
    }
}
