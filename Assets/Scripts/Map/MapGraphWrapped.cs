using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using VRNavigation.MapData;

namespace Map
{
    /// <summary>
    /// Represents a map as a graph. Every node is a location, and every edge is a path.
    /// </summary>
    public class MapGraphWrapped : MapGraph
    {
        /// <summary>
        /// The resource manager for this map.
        /// </summary>
        internal MapResourceManager resourceManager;

        /// <summary>
        /// Create a new map graph.
        /// </summary>
        /// <param name="name">The name of the map.</param>
        /// <param name="nodes">The nodes, as an (id, node) dictionary.</param>
        /// <param name="edges">The edges, as a (source node id, List[edge]) dictionary.</param>
        /// <param name="startPoint">The starting point.</param>
        /// <param name="endPoints">The ending points.</param>
        public MapGraphWrapped(string name, Dictionary<int, MapNode> nodes, Dictionary<int, MapEdge[]> edges,
            int startPoint, HashSet<int> endPoints) : base(name, nodes, edges, startPoint, endPoints)
        {
        }

        /// <summary>
        /// Create a new MapGraphWrapped using an existing map graph.
        /// </summary>
        /// <param name="graph">The map graph to wrap.</param>
        public MapGraphWrapped(MapGraph graph) : base(graph)
        {
        }

        /// <summary>
        /// Run additional setup, as the constructor might not be called.
        /// </summary>
        internal void SetUp()
        {
        }

        /// <summary>
        /// Returns the edges of 'node'.
        /// </summary>
        public IEnumerable<MapEdgeWrapper> EdgesOf(MapNodeWrapper node)
        {
            return edges.TryGetValue(node.ID, out var edgeList)
                ? edgeList.Select(edge => WrapEdge(edge, node.ID))
                : Enumerable.Empty<MapEdgeWrapper>();
        }

        /// <summary>
        /// Returns the destination node of 'edge'.
        /// </summary>
        public MapNodeWrapper NodeOf(MapEdgeWrapper edge)
        {
            return WrapNode(nodes[edge.DestNode], edge.DestNode);
        }

        /// <summary>
        /// The start point node.
        /// </summary>
        public MapNodeWrapper StartPoint => WrapNode(nodes[startPoint], startPoint);

        /// <summary>
        /// Wrap a MapNode into a MapNodeWrapper.
        /// </summary>
        /// <param name="node">The node to wrap.</param>
        /// <param name="id">The node's id.</param>
        /// <returns>The wrapped node.</returns>
        private MapNodeWrapper WrapNode(MapNode node, int id)
        {
            return new MapNodeWrapper(node, id, this, resourceManager);
        }

        /// <summary>
        /// Wrap a MapEdge into a MapEdgeWrapper.
        /// </summary>
        /// <param name="edge">The edge to wrap.</param>
        /// <param name="sourceNodeID">The edge's source node's id.</param>
        /// <returns>The wrapped edge.</returns>
        private MapEdgeWrapper WrapEdge(MapEdge edge, int sourceNodeID)
        {
            return new MapEdgeWrapper(edge, sourceNodeID);
        }

        /// <summary>
        /// Checks if node is an end point.
        /// </summary>
        public bool IsEndPoint(MapNodeWrapper node)
        {
            return endPoints.Contains(node.ID);
        }

        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>A string representing this object.</returns>
        public override string ToString()
        {
            return
                $"{nameof(name)}: {name}, {nameof(nodes)}: {string.Join(", ", nodes)}," +
                $"\n" +
                $"{nameof(edges)}: {string.Join(", ", edges.Select(m => m.Key + " : [" + string.Join("], [", m.Value) + "]"))}," +
                $"\n" +
                $"{nameof(startPoint)}: {startPoint}, {nameof(endPoints)}: {string.Join(", ", endPoints)}";
        }

        /// <summary>
        /// All the nodes in the graph.
        /// </summary>
        internal IEnumerable<MapNodeWrapper> GetAllNodes =>
            nodes.Select(node => WrapNode(node.Value, node.Key));
    }
}