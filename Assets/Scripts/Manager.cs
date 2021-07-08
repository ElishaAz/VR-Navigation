using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Keiwando.NFSO;
using Map;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRNavigation.MapData;

/// <summary>
/// Manages the overall gameplay.
/// </summary>
public class Manager : MonoBehaviour
{
    /// <summary>
    /// Instance.
    /// </summary>
    public static Manager Instance { get; private set; }

    /// <summary>
    /// The locally stored maps and their location.
    /// </summary>
    private Dictionary<MapInfo, string> localMaps;

    /// <summary>
    /// An action to perform when a map is imported.
    /// </summary>
    public Action<MapInfo> onMapImported;


    [SerializeField] [Tooltip("The Map scene.")]
    private string mapScene;

    /// <summary>
    /// The map to load in the map scene.
    /// </summary>
    private MapInfo mapInfoToLoad;

    /// <summary>
    /// Should we load a map on scene load?
    /// </summary>
    private bool loadOnStart;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            FirstTime();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called after every Scene is loaded.
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="loadSceneMode"></param>
    private void OnSceneWasLoaded(Scene arg0, LoadSceneMode loadSceneMode)
    {
        if (loadOnStart)
        {
            LoadMap(mapInfoToLoad);
            VrModeController.Instance.EnterVR();
        }
    }

    /// <summary>
    /// Called the first time this object is instantiated.
    /// </summary>
    private void FirstTime()
    {
        SceneManager.sceneLoaded += OnSceneWasLoaded;

        // locate maps
        localMaps = IOTools.LocateMaps();

        NativeFileSOMobile.shared.FilesWereOpened += delegate(OpenedFile[] files)
        {
            foreach (var file in files)
            {
                var map = IOTools.ImportMap(GlobalVars.LocalMapsLocation, file.Data);
                onMapImported?.Invoke(map.Key);
                break;
            }

            // update local maps
            localMaps = IOTools.LocateMaps(GlobalVars.LocalMapsLocation);
        };
    }

    /// <summary>
    /// Load a map.
    /// </summary>
    /// <param name="info">The map to load.</param>
    /// <param name="contentRoot">The map's content location.</param>
    public void LoadMap(MapInfo info, string contentRoot = null)
    {
        contentRoot ??= localMaps[info];

        var graph = new MapGraphWrapped(IOTools.ReadMap(contentRoot));

        Debug.Log($"Loading map: {info}");

        MapManager.instance.Init(graph, contentRoot);
    }

    /// <summary>
    /// Load a map in the Map scene.
    /// </summary>
    /// <param name="mapInfo">The map to load.</param>
    public void LoadMapInMapScene(MapInfo mapInfo)
    {
        mapInfoToLoad = mapInfo;
        loadOnStart = true;
        SceneManager.LoadScene(mapScene);
    }

    /// <summary>
    /// The locally stored maps.
    /// </summary>
    public IEnumerable<MapInfo> Maps => localMaps.Keys;
}