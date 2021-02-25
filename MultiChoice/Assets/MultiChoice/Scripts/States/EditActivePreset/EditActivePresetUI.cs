////////////////////////////////////////////////////////////
/////   EditActivePresetUI.cs
/////   James McNeil - 2021
////////////////////////////////////////////////////////////

using PersonalFramework;
using TMPro;
using UnityEngine.UI;

public class EditActivePresetUI : UIStateBase
{
    private Button m_editButton = null;
    private TMP_InputField m_presetContent = null;

    protected override void OnAwake()
    {
        m_editButton = gameObject.GetComponentFromChild<Button>("EditPreset");
        m_presetContent = gameObject.GetComponentFromChild<TMP_InputField>("PresetContent");
    }

    public void SetContent(string content)
    {
        m_presetContent.text = content;
    }

    public void SetEditButtonEnabled(bool enabled)
    {
        m_editButton.interactable = enabled;
    }

    public string GetPresetContent()
    {
        return m_presetContent.text;
    }
}
