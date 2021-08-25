using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Keiwando.NFSO;
using Logic;
using Map;
using Newtonsoft.Json;
using UnityEngine;
using VRNavigation.MapData;

/// <summary>
/// A collection of file input and output tools.
/// </summary>
public class IOTools
{
    #region compat

    /// <summary>
    /// Reads an old JSON file.
    /// Copied from the old code.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <returns>A 'Points' object corresponding to the JSON file.</returns>
    public static Points ReadOldJOSN(string path)
    {
        // read the file
        var bytes = File.ReadAllBytes(path);
        var str = System.Text.Encoding.Default.GetString(bytes);

        // deserialize it
        Points points = JsonConvert.DeserializeObject<Points>(str);
        SortByText(ref points);
        return points;
    }

    /// <summary>
    /// A function used by 'ReadOldJOSN'.
    /// Copied from the old code.
    /// </summary>
    private static void SortByText(ref Points p)
    {
        foreach (Point points in p.points)
        {
            points.OptionalText = points.OptionalText.OrderBy(o => o.whenToDisplay).ToList();
        }
    }

    /// <summary>
    /// Translates a 'Points' object to a Map Graph.
    /// </summary>
    /// <param name="points">The 'Points' object.</param>
    /// <returns>An equivalent Map Graph.</returns>
    public static MapGraph PointsToMap(Points points)
    {
        // initialize arguments
        var name = points.Projectname;
        var nodes = new Dictionary<int, MapNode>();
        var edges = new Dictionary<int, MapEdge[]>();
        var startPoint = (int) points.StartPoint;
        var endPoints = new HashSet<int>();

        // for every point
        foreach (var point in points.points)
        {
            var id = point.id;

            // get node
            var node = PointToNode(point);

            // get edge list
            var edgeList = new List<MapEdge>();
            foreach (var neighbor in point.Neighbors)
            {
                edgeList.Add(NeighborToEdge(neighbor));
            }

            // add node
            nodes.Add(id, node);
            // add edge list
            edges.Add(id, edgeList.ToArray());
        }

        // add end points
        foreach (var endPoint in points.EndPoints)
        {
            endPoints.Add((int) endPoint);
        }

        return new MapGraph(name, nodes, edges, startPoint, endPoints);
    }

    /// <summary>
    /// Translates a 'Point' object to a corresponding Map Node.
    /// </summary>
    private static MapNode PointToNode(Point point)
    {
        return new MapNode(point.Picture, OptionalTextsToMapNodeTexts(point.OptionalText));
    }

    private static MapNodeText[] OptionalTextsToMapNodeTexts(List<Optionaltext> optionaltexts)
    {
        MapNodeText[] texts = new MapNodeText[optionaltexts.Count];

        for (int i = 0; i < optionaltexts.Count; i++)
        {
            texts[i] = OptionalTextToMapNodeText(optionaltexts[i]);
        }

        return texts;
    }

    private static MapNodeText OptionalTextToMapNodeText(Optionaltext optionaltext)
    {
        return new MapNodeText(optionaltext.text, optionaltext.whenToDisplay,
            optionaltext.whenToDisplay + optionaltext.DurationInSeconds);
    }

    /// <summary>
    /// Translates a 'Neighbor' object to a corresponding Map Edge.
    /// </summary>
    private static MapEdge NeighborToEdge(Neighbor neighbor)
    {
        return new MapEdge(neighbor.PointID, neighbor.Azimut);
    }

    #endregion

    /// <summary>
    /// Loads a texture from image file.
    /// </summary>
    /// <param name="path">The path to the image file.</param>
    /// <returns>The image as a texture.</returns>
    public static Texture2D LoadImage(string path)
    {
        var filePath = path;

        /* Code copied from old MapBuilder */
        Texture2D tex = new Texture2D(2, 2);
        byte[] bytes = File.ReadAllBytes(filePath);
        tex.LoadImage(bytes);
        return tex;
    }

    /// <summary>
    /// Save a map locally.
    /// </summary>
    /// <param name="mapsStorageLocation">The directory to store the map in.</param>
    /// <param name="data">The zipped map as a byte array.</param>
    /// <param name="map">A (map info, map location) pair.</param>
    /// <returns>True if the map was loaded successfully, false otherwise.</returns>
    public static bool ImportMap(string mapsStorageLocation, byte[] data,
        out KeyValuePair<MapInfo, string> map)
    {
        var extractLocation = GlobalVars.MapExtractionLocation;
        if (Directory.Exists(extractLocation))
        {
            Directory.Delete(extractLocation, true);
        }

        Directory.CreateDirectory(extractLocation);

        using MemoryStream mem = new MemoryStream(data);
        using ZipArchive archive = new ZipArchive(mem);
        archive.ExtractToDirectory(extractLocation);

        if (!CheckMapStructure(extractLocation))
        {
            map = default;
            return false;
        }

        var info = ReadMapInfo(extractLocation);

        if (!Directory.Exists(mapsStorageLocation))
        {
            Directory.CreateDirectory(mapsStorageLocation);
        }

        var mapLocation = GlobalVars.GetMapLocalLocation(info);

        Directory.Move(extractLocation, mapLocation);
        Debug.Log($"Imported {info.name}");
        map = new KeyValuePair<MapInfo, string>(info, mapLocation);
        return true;
    }

    /// <summary>
    /// Locates maps in a given directory according to directory structure.
    /// </summary>
    /// <param name="rootDirectory">The root directory to search.</param>
    /// <returns>A list of paths to root directories of located maps.</returns>
    public static Dictionary<MapInfo, string> LocateMaps(string rootDirectory = null)
    {
        rootDirectory ??= GlobalVars.LocalMapsLocation;

        var maps = new Dictionary<MapInfo, string>();

        if (!Directory.Exists(rootDirectory))
        {
            Directory.CreateDirectory(rootDirectory);
            return maps;
        }

        foreach (var directory in Directory.EnumerateDirectories(rootDirectory))
        {
            if (CheckMapStructure(directory))
            {
                var info = ReadMapInfo(directory);
                maps.Add(info, directory);
            }
        }

        return maps;
    }

    private static bool CheckMapStructure(string directory)
    {
        var hasMapConfig = false;
        var hasMapInfo = false;

        // check if it's a map
        foreach (var file in Directory.EnumerateFiles(directory))
        {
            if (Path.GetFileName(file) == GlobalVars.MapConfigFile)
            {
                hasMapConfig = true;
                if (hasMapInfo) return true;
            }

            if (Path.GetFileName(file) == GlobalVars.MapInfoFile)
            {
                hasMapInfo = true;
                if (hasMapConfig) return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Read a map's info from local storage.
    /// </summary>
    /// <param name="mapContentRoot"></param>
    /// <returns></returns>
    public static MapInfo ReadMapInfo(string mapContentRoot)
    {
        // read the file
        var bytes = File.ReadAllBytes(Path.Combine(mapContentRoot, GlobalVars.MapInfoFile));
        var str = System.Text.Encoding.Default.GetString(bytes);

        // deserialize it
        MapInfo info = JsonConvert.DeserializeObject<MapInfo>(str);

        return info;
    }

    /// <summary>
    /// Read a map from local storage.
    /// </summary>
    /// <param name="mapContentRoot">The root directory of the map.</param>
    /// <returns>The map graph.</returns>
    public static MapGraph ReadMap(string mapContentRoot)
    {
        // read the file
        var bytes = File.ReadAllBytes(Path.Combine(mapContentRoot, GlobalVars.MapConfigFile));
        var str = System.Text.Encoding.Default.GetString(bytes);

        // deserialize it
        MapGraph map = JsonConvert.DeserializeObject<MapGraph>(str);

        return map;
    }
}