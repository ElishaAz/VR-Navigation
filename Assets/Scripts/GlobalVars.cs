using System.IO;
using UnityEngine;
using VRNavigation.MapData;

public class GlobalVars
{
    /// <summary>
    /// The path to the local storage location for the maps.
    /// </summary>
    public static string LocalMapsLocation => Path.Combine(Application.persistentDataPath, "Maps");

    /// <summary>
    /// The path to temporary extraction location.
    /// </summary>
    public static string MapExtractionLocation => Path.Combine(Application.persistentDataPath, "temp", "extracted");

    /// <summary>
    /// The map configuration file's name.
    /// </summary>
    public const string MapConfigFile = "map.config";

    /// <summary>
    /// The map info file's name.
    /// </summary>
    public const string MapInfoFile = "map.info";

    /// <summary>
    /// Gets the local location for a map.
    /// </summary>
    /// <param name="info">The map in question.</param>
    /// <returns>The path to the local location for the map.</returns>
    public static string GetMapLocalLocation(MapInfo info)
    {
        return Path.Combine(LocalMapsLocation, $"{info.name}v{info.version}");
    }
}