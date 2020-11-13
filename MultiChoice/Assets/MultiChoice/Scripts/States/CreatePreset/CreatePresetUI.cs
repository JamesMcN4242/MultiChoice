////////////////////////////////////////////////////////////
/////   CreatePresetUI.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreatePresetUI : UIStateBase
{
    private Button m_createButton = null;
    private TMP_InputField m_presetName = null;
    private TMP_InputField m_presetContent = null;

    protected override void OnAwake()
    {
        m_createButton = gameObject.GetComponentFromChild<Button>("CreatePreset");
        m_presetName = gameObject.GetComponentFromChild<TMP_InputField>("PresetName");
        m_presetContent = gameObject.GetComponentFromChild<TMP_InputField>("PresetContent");
    }

    public void AddPresetListener(UnityAction<string> listener)
    {
        m_presetName.onValueChanged.AddListener(listener);
    }

    public void ClearPresetListeners()
    {
        m_presetName.onValueChanged.RemoveAllListeners();
    }

    public void SetNameValidated(bool valid)
    {
        if (valid)
        {
            m_presetName.colors = ColorBlock.defaultColorBlock;
        }
        else
        {
            ColorBlock warningBlock = new ColorBlock();
            Color red = Color.red;

            warningBlock.selectedColor = red;
            warningBlock.disabledColor = red;
            warningBlock.highlightedColor = red;
            warningBlock.normalColor = red;
            warningBlock.pressedColor = red;

            m_presetName.colors = warningBlock;
        }
    }

    public void SetCreateButtonEnabled(bool enabled)
    {
        m_createButton.interactable = enabled;
    }

    public string GetPresetName()
    {
        return m_presetName.text;
    }

    public string GetPresetContent()
    {
        return m_presetContent.text;
    }
}
