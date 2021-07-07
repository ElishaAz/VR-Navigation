using VRNavigation.MapData;

namespace Map
{
    /// <summary>
    /// An edge in the map, representing a path from one location to another (not bidirectional).
    /// </summary>
    public struct MapEdgeWrapper
    {
        /// <summary>
        /// The edge we're wrapping.
        /// </summary>
        private MapEdge edge;


        /// <summary>
        /// The id of the source node of this edge.
        /// </summary>
        private int sourceNodeID;

        /// <summary>
        /// Create a new map edge.
        /// </summary>
        /// <param name="edge">The edge to wrap.</param>
        /// <param name="sourceNodeID">The id of the source node.</param>
        public MapEdgeWrapper(MapEdge edge, int sourceNodeID)
        {
            this.edge = edge;
            this.sourceNodeID = sourceNodeID;
        }

        /// <summary>
        /// The azimuth between the source node and the target node.
        /// </summary>
        public float Azimuth => edge.azimuth;

        /// <summary>
        /// The id of the destination node of this edge.
        /// </summary>
        internal int DestNode => edge.node;

        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>A string representing this object.</returns>
        public override string ToString()
        {
            return $"{nameof(edge)}: {edge}, {nameof(sourceNodeID)}: {sourceNodeID}";
        }
    }
}