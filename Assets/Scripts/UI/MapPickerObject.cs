using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRNavigation.MapData;

public class MapPickerObject : MonoBehaviour
{
    [SerializeField] private MapButton buttonPrefab;
    [SerializeField] private GameObject noMapsText;

    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector2 spacing;
    [SerializeField] private int rows = 5;

    private int currentRow, currentColumn;

    private void Start()
    {
        CreateButtonGrid();
        Manager.Instance.onMapImported += CreateButton;
    }

    private void CreateButtonGrid()
    {
        noMapsText.SetActive(true);
        var maps = Manager.Instance.Maps;
        foreach (var map in maps)
        {
            CreateButton(map);
        }
    }

    private void CreateButton(MapInfo info)
    {
        currentRow++;
        if (currentRow >= rows)
        {
            currentRow = 0;
            currentColumn++;
        }

        noMapsText.SetActive(false);
        var position = startPoint +
                       Vector3.right * currentRow * spacing.x +
                       Vector3.down * currentColumn * spacing.y;
        var button = Instantiate(buttonPrefab, position, Quaternion.identity);
        button.SetUp(info, OnClick(info));
        Debug.Log("Added button");
    }

    private static UnityAction OnClick(MapInfo info)
    {
        return () => { Manager.Instance.LoadMapInMapScene(info); };
    }
}