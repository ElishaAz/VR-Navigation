using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Resizes the <see cref="GridLayoutGroup"/>'s cell size based on a ratio.
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup))]
public class RatioCellsGridLayout : MonoBehaviour
{
    /// <summary>
    /// The ratio of the layout the cells should occupy.
    /// </summary>
    [SerializeField] private Vector2 ratio;

    /// <summary>
    /// The GridLayoutGroup that is edited.
    /// </summary>
    private GridLayoutGroup gridLayout;

    /// <summary>
    /// The RectTransform for getting the size.
    /// </summary>
    private RectTransform rectTransform;

    /// <summary>
    /// The previous rect size (for identifying change).
    /// </summary>
    private Vector2 lastRectSize;

    /// <summary>
    /// The previous ratio (for identifying change).
    /// </summary>
    private Vector2 lastRatio;

    private void Awake()
    {
        lastRatio = ratio;
        gridLayout = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();

        UpdateCellSize();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Application.isEditor && !Application.isPlaying)
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }

            if (gridLayout == null)
            {
                gridLayout = GetComponent<GridLayoutGroup>();
            }
        }
#endif

        // if there was a change
        if (ratio != lastRatio || rectTransform.rect.size != lastRectSize)
        {
            UpdateCellSize();
        }
    }

    private void UpdateCellSize()
    {
        // update the cell size
        lastRatio = ratio;
        lastRectSize = rectTransform.rect.size;
        gridLayout.cellSize = lastRectSize * ratio;
    }
}