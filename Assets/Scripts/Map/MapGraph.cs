using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Map
{
    /// <summary>
    /// Represents a map as a graph. Every node is a location, and every edge is a path.
    /// </summary>
    [DataContract]
    public class MapGraph
    {
        private string name;

        /// <summary>
        /// All the nodes, as a dictionary of the ids and nodes.
        /// </summary>
        [DataMember] private Dictionary<int, MapNode> nodes;

        /// <summary>
        /// All the edges, as a dictionary of the source nodes' ids and a list of edges from that node.
        /// </summary>
        [DataMember] private Dictionary<int, MapEdge[]> edges;


        /// <summary>
        /// The starting point.
        /// </summary>
        [DataMember] private int startPoint;

        /// <summary>
        /// The ending points.
        /// </summary>
        [DataMember] private HashSet<int> endPoints;

        /// <summary>
        /// The resource manager for this map.
        /// </summary>
        internal IResourceManager resourceManager;


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
        /// Run additional setup, as the constructor might not be called.
        /// </summary>
        internal void SetUp()
        {
            foreach (var id in nodes.Keys.ToList())
            {
                nodes[id] = AssignNodeValues(nodes[id], id);
            }
        }

        /// <summary>
        /// Returns the edges of 'node'.
        /// </summary>
        public MapEdge[] EdgesOf(MapNode node)
        {
            return edges.TryGetValue(node.id, out var edgeList) ? edgeList : new MapEdge[] { };
        }

        /// <summary>
        /// Returns the target node of 'edge'.
        /// </summary>
        public MapNode NodeOf(MapEdge edge)
        {
            return AssignNodeValues(nodes[edge.node], edge.node);
        }

        /// <summary>
        /// The start point node.
        /// </summary>
        public MapNode StartPoint => AssignNodeValues(nodes[startPoint], startPoint);

        /// <summary>
        /// Assigns the node's non-constructor values.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="id">The node's id.</param>
        /// <returns>The node with the assigned values.</returns>
        private MapNode AssignNodeValues(MapNode node, int id)
        {
            node.id = id;
            node.parentGraph = this;
            node.resourceManager = resourceManager;
            return node;
        }

        /// <summary>
        /// Checks if node is an end point.
        /// </summary>
        public bool IsEndPoint(MapNode node)
        {
            return endPoints.Contains(node.id);
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

        internal ICollection<MapNode> GetAllNodes => nodes.Values;
    }
}