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

    [SerializeField] private TMPro.TMP_Text textField;

    private UnityAction onButtonClick;

    public void SetUp(MapInfo info, UnityAction onButtonClick)
    {
        // set values
        this.info = info;
        mapName = info.name;
        version = info.version;
        this.onButtonClick = onButtonClick;

        textField.text = $"{mapName}\nv{version}";
    }

    public void Click()
    {
        onButtonClick?.Invoke();
    }
}