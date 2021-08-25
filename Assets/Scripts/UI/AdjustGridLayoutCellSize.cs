using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Code from: https://forum.unity.com/threads/solved-how-to-make-grid-layout-group-cell-size-x-auto-expand.448534/
/// </summary>
[ExecuteInEditMode]
// [RequireComponent(typeof(GridLayoutGroup))]
public class AdjustGridLayoutCellSize : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y
    };

    public enum RatioMode
    {
        Free,
        Fixed
    };

    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private Axis expand;
    [SerializeField] private RatioMode ratioMode;
    [SerializeField] private float cellRatio = 1;

    private new RectTransform transform;

    private void Awake()
    {
        transform = (RectTransform) base.transform;
        // grid = GetComponent<GridLayoutGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateCellSize();
    }

    void OnRectTransformDimensionsChange()
    {
        UpdateCellSize();
    }

#if UNITY_EDITOR
    [ExecuteAlways]
    void Update()
    {
        UpdateCellSize();
    }
#endif

    private void OnValidate()
    {
        transform = (RectTransform) base.transform;
        // grid = GetComponent<GridLayoutGroup>();
        UpdateCellSize();
    }

    public void UpdateCellSize()
    {
        if (grid is null) return;

        if (transform == null)
            transform = (RectTransform) base.transform;

        var count = grid.constraintCount;
        if (expand == Axis.X)
        {
            float spacing = (count - 1) * grid.spacing.x;
            float contentSize = transform.rect.width - grid.padding.left - grid.padding.right - spacing;
            float sizePerCell = contentSize / count;
            grid.cellSize = new Vector2(sizePerCell,
                ratioMode == RatioMode.Free ? grid.cellSize.y : sizePerCell * cellRatio);
        }
        else //if (expand == Axis.Y)
        {
            float spacing = (count - 1) * grid.spacing.y;
            float contentSize = transform.rect.height - grid.padding.top - grid.padding.bottom - spacing;
            float sizePerCell = contentSize / count;
            grid.cellSize = new Vector2(ratioMode == RatioMode.Free ? grid.cellSize.x : sizePerCell * cellRatio,
                sizePerCell);
        }
    }
}