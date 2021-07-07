using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace VRNavigation.MapData
{
    /// <summary>
    /// Represents a map as a graph. Every node is a location, and every edge is a path.
    /// </summary>
    [DataContract]
    public class MapGraph
    {
        /// <summary>
        /// The name of this map.
        /// </summary>
        [DataMember] public readonly string name;

        /// <summary>
        /// All the nodes, as a dictionary of the ids and nodes.
        /// </summary>
        [DataMember] protected Dictionary<int, MapNode> nodes;

        /// <summary>
        /// All the edges, as a dictionary of the source nodes' ids and a list of edges from that node.
        /// </summary>
        [DataMember] protected Dictionary<int, MapEdge[]> edges;

        /// <summary>
        /// The starting point.
        /// </summary>
        [DataMember] protected int startPoint;

        /// <summary>
        /// The ending points.
        /// </summary>
        [DataMember] protected HashSet<int> endPoints;


        /// <summary>
        /// Create a new map graph.
        /// </summary>
        /// <param name="name">The name of the map.</param>
        /// <param name="nodes">The nodes, as a (id, node) dictionary.</param>
        /// <param name="edges">The edges, as a (source node id, List[edge]) dictionary.</param>
        /// <param name="startPoint">The starting point.</param>
        /// <param name="endPoints">The ending points.</param>
        public MapGraph(string name, Dictionary<int, MapNode> nodes, Dictionary<int, MapEdge[]> edges,
            int startPoint, HashSet<int> endPoints)
        {
            this.name = name;
            this.nodes = nodes;
            this.edges = edges;
            this.startPoint = startPoint;
            this.endPoints = endPoints;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">The map graph to copy.</param>
        public MapGraph(MapGraph other)
        {
            this.name = other.name;
            this.nodes = other.nodes;
            this.edges = other.edges;
            this.startPoint = other.startPoint;
            this.endPoints = other.endPoints;
        }

        public override string ToString()
        {
            return
                $"{nameof(name)}: {name}, {nameof(nodes)}: {string.Join(", ", nodes)}," +
                $"\n" +
                $"{nameof(edges)}: {string.Join(", ", edges.Select(m => m.Key + " : [" + string.Join("], [", m.Value) + "]"))}," +
                $"\n" +
                $"{nameof(startPoint)}: {startPoint}, {nameof(endPoints)}: {string.Join(", ", endPoints)}";
        }
    }
}