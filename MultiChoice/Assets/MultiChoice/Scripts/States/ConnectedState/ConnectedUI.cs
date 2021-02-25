using PersonalFramework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectedUI : UIStateBase
{
    private const int k_elementsInRow = 6;
    private const int k_elementsInColumn = 4;
    private const float k_rowSpacing = 0.03f;
    private const float k_columnSpacing = 0.03f;
    private const float k_rowElementSize = (1.0f / k_elementsInRow) - k_columnSpacing;
    private const float k_columnElementSize = (1.0f / k_elementsInColumn) - k_rowSpacing;
    public const int k_elementsPerGrid = k_elementsInColumn * k_elementsInRow;

    private Transform m_gridTransform = null;
    private Image[] m_elementImages = null;
    private TextMeshProUGUI m_codeText = null;
    private GameObject m_selectButton = null;

    private void Start()
    {
        m_gridTransform = gameObject.FindChildByName("Grid").transform;
        m_codeText = gameObject.GetComponentFromChild<TextMeshProUGUI>("ConnectionCode");
        m_selectButton = transform.Find("Content/Select").gameObject;
    }

    //TODO: Just a make a generic grid UI element that can be attached instead of copying this over from other sections
    public void BuildGridElements(List<string> gridElements, int pageNumber)
    {
        ClearGridElements();
        m_elementImages = new Image[gridElements.Count];
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

                TextMeshProUGUI text = element.gameObject.GetComponentFromChild<TextMeshProUGUI>("Text");
                text.text = gridElements[gridIndex];

                m_elementImages[gridIndex] = element.GetComponent<Image>();

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

    public void ClearGridElements()
    {
        m_gridTransform.DestroyAllChildren();
    }

    public void SetElementColour(int elementIndex, Color color)
    {
        m_elementImages[elementIndex].color = color;
    }

    public void SetConnectionCode(string[] code)
    {
        m_codeText.text = string.Join(" ", code);
    }

    public void SetSelectionButtonEnabled(bool enabled)
    {
        m_selectButton.SetActive(enabled);
    }
}
