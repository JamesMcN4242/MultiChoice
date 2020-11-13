////////////////////////////////////////////////////////////
/////   UIConfirmationPopup.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////
///
/// NOTE: This class is taken directly from another of my project's that can be found here
/// https://github.com/JamesMcN4242/BudgetingApp/blob/master/Assets/BudgetApp/Scripts/States/ConfirmationPopup/UIConfirmationPopup.cs

using PersonalFramework;
using TMPro;

public class UIConfirmationPopup : UIStateBase
{
    private TextMeshProUGUI m_title = null;
    private TextMeshProUGUI m_description = null;
    private TextMeshProUGUI m_confirmation = null;
    private TextMeshProUGUI m_decline = null;

    void Start()
    {
        m_title = gameObject.GetComponentFromChild<TextMeshProUGUI>("Title");
        m_description = gameObject.GetComponentFromChild<TextMeshProUGUI>("Description");
        m_confirmation = gameObject.GetComponentFromChild<TextMeshProUGUI>("ConfirmText");
        m_decline = gameObject.GetComponentFromChild<TextMeshProUGUI>("DeclineText");
    }

    public void SetText(ConfirmationPopupState.PopupText popupText)
    {
        m_title.text = popupText.m_title;
        m_description.text = popupText.m_description;
        m_confirmation.text = popupText.m_accept;
        m_decline.text = popupText.m_decline;
    }
}
