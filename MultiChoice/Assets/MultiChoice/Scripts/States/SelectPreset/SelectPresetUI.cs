////////////////////////////////////////////////////////////
/////   SelectPresetUI.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////
///
/// NOTE: This class is an adapted version of my UIIncomeExpenses class from another project that can be found here
/// https://github.com/JamesMcN4242/BudgetingApp/blob/master/Assets/BudgetApp/Scripts/States/IncomeExpensesManagement/UIIncomeExpenses.cs


using PersonalFramework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPresetUI : UIStateBase
{
    private static readonly Color k_selectedColor = Color.yellow;

    private const int k_elementsInRow = 6;
    private const int k_elementsInColumn = 4;
    private const float k_rowSpacing = 0.03f;
    private const float k_columnSpacing = 0.03f;
    private const float k_rowElementSize = (1.0f / k_elementsInRow) - k_columnSpacing;
    private const float k_columnElementSize = (1.0f / k_elementsInColumn) - k_rowSpacing;
    public const int k_elementsPerGrid = k_elementsInColumn * k_elementsInRow;
    public const string k_selectElementMsg = "selected_{0}";

    private Transform m_gridTransform = null;
    private Button m_removeButton = null;
    private Button m_previousPage = null;
    private Button m_nextPage = null;

    void Start()
    {
        m_gridTransform = gameObject.FindChildByName("Grid").transform;
        m_removeButton = gameObject.GetComponentFromChild<Button>("Remove");
        m_previousPage = gameObject.GetComponentFromChild<Button>("PreviousPage");
        m_nextPage = gameObject.GetComponentFromChild<Button>("NextPage");
    }

    public void BuildGridElements(List<string> gridElements, int pageNumber)
    {
        m_gridTransform.DestroyAllChildren();
        if (gridElements == null || gridElements.Count == 0) return;

        GameObject gridPrefab = Resources.Load<GameObject>("UIGridElement");
        float yMaxAnchor = 1.0f;
        for (int rowIndex = 0, gridIndex = pageNumber * k_elementsPerGrid; rowIndex < k_elementsInColumn; rowIndex++)
        {
            float xMinAnchor = 0.0f;
            for (int columnIndex = 0; columnIndex < k_elementsInRow; columnIndex++)
            {
                RectTransform element = Object.Instantiate(gridPrefab, m_gridTransform).GetComponent<RectTransform>();
                element.name = $"Element_{gridIndex}";

                UIButtonInteraction buttonInteraction = element.GetComponent<UIButtonInteraction>();
                buttonInteraction.m_message = k_selectElementMsg + gridIndex;

                TextMeshProUGUI text = element.gameObject.GetComponentFromChild<TextMeshProUGUI>("Text");
                text.text = gridElements[gridIndex];

                element.anchorMin = new Vector2(xMinAnchor, yMaxAnchor - k_columnElementSize);
                element.anchorMax = new Vector2(xMinAnchor + k_rowElementSize, yMaxAnchor);

                xMinAnchor += k_rowElementSize + k_rowSpacing;
                gridIndex++;
                if (gridIndex >= gridElements.Count)
                {
                    return;
                }
            }
            yMaxAnchor -= k_columnElementSize + k_columnSpacing;
        }
    }

    public void SetRemoveInteractablity(bool interactable)
    {
        m_removeButton.interactable = interactable;
    }

    public void SetPageNavigatorInteractability(bool previousPageInteractable, bool nextPageInteractable)
    {
        m_previousPage.interactable = previousPageInteractable;
        m_nextPage.interactable = nextPageInteractable;
    }

    public void SetButtonSelected(int indexToSelect, int previousIndex = -1)
    {
        Image toSelectImage = gameObject.GetComponentFromChild<Image>($"Element_{indexToSelect}");
        toSelectImage.color = k_selectedColor;

        if (previousIndex > -1)
        {
            Image toUnselectImage = gameObject.GetComponentFromChild<Image>($"Element_{previousIndex}");
            toUnselectImage.color = Color.white;
        }
    }
}
