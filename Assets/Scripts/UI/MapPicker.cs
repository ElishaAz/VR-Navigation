using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRNavigation.MapData;

public class MapPicker : MonoBehaviour
{
    [SerializeField] private MapButton buttonPrefab;
    [SerializeField] private LayoutGroup buttonGroup;

    private void Awake()
    {
        CreateButtonGrid();
        Manager.Instance.onMapImported += CreateButton;
    }

    private void CreateButtonGrid()
    {
        var maps = Manager.Instance.Maps;
        foreach (var map in maps)
        {
            CreateButton(map);
        }
    }

    private void CreateButton(MapInfo info)
    {
        var button = Instantiate(buttonPrefab, buttonGroup.transform, true);
        button.SetUp(info, OnClick(info));
    }

    private static UnityAction OnClick(MapInfo info)
    {
        return () => { Manager.Instance.LoadMapInMapScene(info); };
    }
}