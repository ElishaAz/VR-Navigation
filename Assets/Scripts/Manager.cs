using System;
using Map;
using UnityEngine;

/// <summary>
/// Manages the overall gameplay.
/// </summary>
public class Manager : MonoBehaviour
{
    private void Start()
    {
        var path = Application.persistentDataPath + "/Json/config.json";
        var graph = IOTools.PointsToMap(IOTools.ReadOldJOSN(path));
        MapManager.instance.Init(graph);
    }
}