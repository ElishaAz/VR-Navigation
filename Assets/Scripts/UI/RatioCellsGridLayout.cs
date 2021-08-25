using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup))]
public class RatioCellsGridLayout : MonoBehaviour
{
    [SerializeField] private Vector2 ratio;
    private GridLayoutGroup gridLayout;
    private RectTransform gridLayoutTransform;

    private Vector2 lastRectSize;
    private Vector2 lastRatio;

    private void Awake()
    {
        lastRatio = ratio;
        gridLayout = GetComponent<GridLayoutGroup>();
        gridLayoutTransform = GetComponent<RectTransform>();

        lastRectSize = gridLayoutTransform.rect.size;
        gridLayout.cellSize = lastRectSize * ratio;
    }

    private void Update()
    {
        if (ratio != lastRatio || gridLayoutTransform.rect.size != lastRectSize)
        {
            lastRatio = ratio;
            lastRectSize = gridLayoutTransform.rect.size;
            gridLayout.cellSize = lastRectSize * ratio;
        }
    }
}