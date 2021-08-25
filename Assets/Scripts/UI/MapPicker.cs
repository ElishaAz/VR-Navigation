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

    [SerializeField] private GameObject noMapsText;

    private void Awake()
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
        noMapsText.SetActive(false);
        var button = Instantiate(buttonPrefab, buttonGroup.transform, true);
        button.SetUp(info, OnClick(info));
        Debug.Log("Added button");
    }

    private static UnityAction OnClick(MapInfo info)
    {
        return () => { Manager.Instance.LoadMapInMapScene(info); };
    }
}