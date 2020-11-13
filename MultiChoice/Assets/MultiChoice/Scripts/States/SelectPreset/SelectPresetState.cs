////////////////////////////////////////////////////////////
/////   SelectPresetState.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////
///
/// NOTE: This class is an adapted version of my IncomeExpensesState class from another project that can be found here
/// https://github.com/JamesMcN4242/BudgetingApp/blob/master/Assets/BudgetApp/Scripts/States/IncomeExpensesManagement/IncomeExpensesState.cs

using PersonalFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectPresetState : FlowStateBase
{
    private const string k_nextPageMsg = "nextPage";
    private const string k_previousPageMsg = "previousPage";
    private const string k_editMsg = "edit";
    private const string k_removeMsg = "remove";

    private SelectPresetUI m_selectPresetUI = null;
    private Dictionary<string, List<string>> m_presets = null;
    private List<string> m_keys = null;
    private Action<string> m_onKeySelected = null;
    
    private int m_pageNumber = 0;
    private int m_selectedElementIndex = -1;

    public SelectPresetState(Dictionary<string, List<string>> presets, Action<string> selectedIdCallback = null)
    {
        m_presets = presets;

        m_keys = new List<string>(m_presets.Count);
        foreach(string key in m_presets.Keys)
        {
            m_keys.Add(key);
        }

        m_onKeySelected = selectedIdCallback;
    }

    protected override void StartPresentingState()
    {
        BuildGridElements();
    }

    public override void ReenteredForeground()
    {
        if(m_keys.Count <= 0)
        {
            ControllingStateStack.PopState(this);
        }
    }

    protected override bool AquireUIFromScene()
    {
        m_selectPresetUI = GameObject.FindObjectOfType<SelectPresetUI>();
        m_ui = m_selectPresetUI;
        return m_ui != null;
    }

    protected override void HandleMessage(object message)
    {
        switch (message)
        {
            case k_editMsg:
                string key = m_keys[m_selectedElementIndex];
                string body = string.Join(", ", m_presets[key]);
                CreatePresetState createState = new CreatePresetState(m_presets, false, key, body);
                createState.AddOnStateDismissingAction(OnEditedValue);
                ControllingStateStack.PushState(createState);
                break;

            case k_removeMsg:
                var popupText = new ConfirmationPopupState.PopupText
                {
                    m_title = "Confirm Deletion",
                    m_description = "Are you sure you want to delete this?",
                    m_accept = "Yes",
                    m_decline = "No"
                };
                ControllingStateStack.PushState(new ConfirmationPopupState(popupText, RemoveSelectedElement));
                break;

            case k_nextPageMsg:
                m_pageNumber++;
                BuildGridElements();
                break;

            case k_previousPageMsg:
                m_pageNumber = Mathf.Max(m_pageNumber - 1, 0);
                BuildGridElements();
                break;

            case string msg when msg.StartsWith(SelectPresetUI.k_selectElementMsg):
                msg = msg.Replace(SelectPresetUI.k_selectElementMsg, string.Empty);
                if (int.TryParse(msg, out int index))
                {
                    if (index == m_selectedElementIndex) break;

                    m_selectPresetUI.SetButtonSelected(index, m_selectedElementIndex);
                    m_selectedElementIndex = index;
                    m_selectPresetUI.SetNonPageButtonInteractablity(true);
                }
                break;

            case "selectPreset":
                m_onKeySelected?.Invoke(m_keys[m_selectedElementIndex]);
                ControllingStateStack.PopState(this);
                break;

            case "back":
                ControllingStateStack.PopState(this);
                break;
        }
    }

    private void OnEditedValue()
    {
        string previousKey = m_keys[m_selectedElementIndex];
        if (m_presets.ContainsKey(previousKey) == false)
        {
            var firstEntry = m_presets.FirstOrDefault(set => m_keys.Contains(set.Key) == false);
            m_keys[m_selectedElementIndex] = firstEntry.Key;
            BuildGridElements();
        }
    }

    private void RemoveSelectedElement()
    {
        string key = m_keys[m_selectedElementIndex];
        m_keys.RemoveAt(m_selectedElementIndex);
        m_presets.Remove(key);

        m_selectedElementIndex = -1;
        SaveToPlayerPrefs();

        if (m_keys.Count > 0)
        {
            int totalPages = (m_keys.Count - 1) / SelectPresetUI.k_elementsPerGrid;
            m_pageNumber = Mathf.Max(Mathf.Min(m_pageNumber, totalPages), 0);

            //TODO: Delete singular element instead of rebuilding full grid
            BuildGridElements();
        }
    }

    private void BuildGridElements()
    {
        m_selectPresetUI.BuildGridElements(m_keys, m_pageNumber);
        RebuildObserverList();

        m_selectedElementIndex = -1;
        m_selectPresetUI.SetNonPageButtonInteractablity(false);
        m_selectPresetUI.SetPageNavigatorInteractability(m_pageNumber > 0, HasAnotherPage());
    }

    private bool HasAnotherPage()
    {
        return m_keys.Count > (m_pageNumber + 1) * SelectPresetUI.k_elementsPerGrid;
    }

    private void SaveToPlayerPrefs()
    {
        PresetDataSystem.SaveNewPresets(m_presets);
    }
}
