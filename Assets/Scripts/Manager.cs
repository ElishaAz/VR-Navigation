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

    [SerializeField] [Tooltip("The Menu scene.")]
    private string menuScene;

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

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            LoadPreviousScene();
        }
    }

    /// <summary>
    /// Called after every Scene is loaded.
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="loadSceneMode"></param>
    private void OnSceneWasLoaded(Scene arg0, LoadSceneMode loadSceneMode)
    {
        if (SceneManager.GetActiveScene().name == mapScene)
        {
            if (loadOnStart)
            {
                LoadMap(mapInfoToLoad);
                VrModeController.Instance.EnterVR();
            }
        }
    }

    /// <summary>
    /// Called the first time this object is instantiated.
    /// </summary>
    private void FirstTime()
    {
        SceneManager.sceneLoaded += OnSceneWasLoaded;

        // locate maps
        UpdateLocalMaps();

        NativeFileSOMobile.shared.FilesWereOpened += delegate(OpenedFile[] files)
        {
            foreach (var file in files)
            {
                ImportMapFile(file.Data);
                break;
            }

            // update local maps
            UpdateLocalMaps();
        };
    }

    public void UpdateLocalMaps()
    {
        localMaps = IOTools.LocateMaps();
    }

    /// <summary>
    /// Import a map file to local storage.
    /// </summary>
    /// <param name="data">The file as a byte array.</param>
    public void ImportMapFile(byte[] data)
    {
        if (IOTools.ImportMap(GlobalVars.LocalMapsLocation, data, out var map))
            onMapImported?.Invoke(map.Key);
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

        MapManager.Instance.Init(graph, contentRoot);
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

    private void LoadMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    public void LoadPreviousScene()
    {
        if (SceneManager.GetActiveScene().name == mapScene)
        {
            VrModeController.Instance.ExitVR();
            LoadMenu();
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    /// <summary>
    /// The locally stored maps.
    /// </summary>
    public IEnumerable<MapInfo> Maps => localMaps.Keys;
}