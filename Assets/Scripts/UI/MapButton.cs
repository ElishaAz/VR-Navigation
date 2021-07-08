using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRNavigation.MapData;

public class MapButton : MonoBehaviour
{
    private MapInfo info;
    [SerializeField] private string mapName;
    [SerializeField] private float version;

    [SerializeField] private Button button;
    [SerializeField] private TMPro.TMP_Text textField;

    public void SetUp(MapInfo info, UnityAction onButtonClick)
    {
        // set values
        this.info = info;
        mapName = info.name;
        version = info.version;
        button.onClick.AddListener(onButtonClick);

        textField.text = $"{mapName}\nv{version}";
    }
}