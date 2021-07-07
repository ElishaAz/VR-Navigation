using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        return new MapNode(point.Picture);
    }

    /// <summary>
    /// Translates a 'Neighbor' object to a corresponding Map Edge.
    /// </summary>
    private static MapEdge NeighborToEdge(Neighbor neighbor)
    {
        return new MapEdge(neighbor.PointID, neighbor.Azimut);
    }
}