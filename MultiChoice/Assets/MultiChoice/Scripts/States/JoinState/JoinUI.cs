////////////////////////////////////////////////////////////
/////   JoinUI.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using PersonalFramework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinUI : UIStateBase
{
    private Button m_startButton = null;
    private TMP_InputField[] m_inputFields = null;

    protected override void OnAwake()
    {
        m_startButton = gameObject.GetComponentFromChild<Button>("Join");
        Transform inputParent = gameObject.FindChildByName("TextInputs").transform;
        m_inputFields = new TMP_InputField[inputParent.childCount];

        for(int i = 0; i < m_inputFields.Length; i++)
        {
            m_inputFields[i] = inputParent.GetChild(i).GetComponent<TMP_InputField>();
        }
    }

    public void SetStartButtonInteractive(bool interactive)
    {
        m_startButton.interactable = interactive;
    }

    public bool DoAllFieldsContainText()
    {
        foreach(TMP_InputField input in m_inputFields)
        {
            if(input.text.Length < 2)
            {
                return false;
            }
        }

        return true;
    }

    public string[] GetCodeInput()
    {
        return Array.ConvertAll(m_inputFields, inputField => inputField.text);        
    }

    public void ClearAllInput()
    {
        foreach (TMP_InputField input in m_inputFields)
        {
            input.text = string.Empty;
        }
    }
}
