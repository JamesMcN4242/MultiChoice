////////////////////////////////////////////////////////////
/////   HostUI.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostUI : UIStateBase
{
    private Button m_selectPresetButton = null;
    private Button m_startButton = null;
    private TextMeshProUGUI m_presetSelectedText = null;

    protected override void OnAwake()
    {
        m_selectPresetButton = gameObject.GetComponentFromChild<Button>("SelectPreset");
        m_startButton = gameObject.GetComponentFromChild<Button>("Start");
        m_presetSelectedText = gameObject.GetComponentFromChild<TextMeshProUGUI>("PresetSelected");
    }

    public void SetPresetSelectionEnabled(bool enabled)
    {
        m_selectPresetButton.interactable = enabled;
    }
    public void SetStartOptionEnabled(bool enabled)
    {
        m_startButton.interactable = enabled;
    }

    public void SetSelectedPreset(string presetName)
    {
        if (string.IsNullOrEmpty(presetName))
        {
            m_presetSelectedText.color = Color.red;
            m_presetSelectedText.text = "No selected preset";
        }
        else
        {
            m_presetSelectedText.color = Color.white;
            m_presetSelectedText.text = $"Selected the preset named: {presetName}";
        }
    }
}
