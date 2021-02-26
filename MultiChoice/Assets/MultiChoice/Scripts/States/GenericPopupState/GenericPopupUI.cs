////////////////////////////////////////////////////////////
/////   GenericPopupUI.cs
/////   James McNeil - 2021
////////////////////////////////////////////////////////////

using PersonalFramework;
using TMPro;

public class GenericPopupUI : UIStateBase
{
    private const string k_popupHolderPath = "Content/PopupBackground/";

    private TextMeshProUGUI m_textBox = null;
    private TextMeshProUGUI m_buttonText = null;

    protected override void OnAwake()
    {
        m_textBox = transform.Find($"{k_popupHolderPath}MessageText").GetComponent<TextMeshProUGUI>();
        m_buttonText = transform.Find($"{k_popupHolderPath}Dismiss/DismissText").GetComponent<TextMeshProUGUI>();
    }

    public void SetPopupText(string text)
    {
        m_textBox.text = text;
    }

    public void SetButtonText(string text)
    {
        m_buttonText.text = text;
    }
}
