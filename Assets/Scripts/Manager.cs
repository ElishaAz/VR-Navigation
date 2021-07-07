using System;
using System.IO;
using System.IO.Compression;
using Keiwando.NFSO;
using Map;
using UnityEngine;

/// <summary>
/// Manages the overall gameplay.
/// </summary>
public class Manager : MonoBehaviour
{
    private static string mapsPath;

    private string currentMapPath;

    private void Awake()
    {
        mapsPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "maps";
        NativeFileSOMobile.shared.FilesWereOpened += delegate(OpenedFile[] files) { ImportMap(files); };
    }

    private void Start()
    {
        var path = Application.persistentDataPath + "/Json/config.json";
        var graph = new MapGraphWrapped(IOTools.PointsToMap(IOTools.ReadOldJOSN(path)));

        currentMapPath = Application.persistentDataPath; // TODO: fix

        MapManager.instance.Init(graph, currentMapPath);
    }

    private void ImportMap(OpenedFile[] files)
    {
        if (files.Length < 1) return;

        var file = files[0];
        var bytes = file.Data;
        var pathToMap = mapsPath + Path.DirectorySeparatorChar + file.Name;

        if (!Directory.Exists(pathToMap))
        {
            Directory.CreateDirectory(pathToMap);
            using (MemoryStream mem = new MemoryStream(bytes))
            using (ZipArchive archive = new ZipArchive(mem))
            {
                archive.ExtractToDirectory(pathToMap);
                currentMapPath = pathToMap;
            }
        }
    }
}