namespace Map
{
    /// <summary>
    /// An edge in the map, representing a path from one location to another (not bidirectional).
    /// </summary>
    public struct MapEdge
    {
        internal int node; // neighbor node's id.
        private float azimuth; // direction angle in degrees.

        /// <summary>
        /// Create a new map edge.
        /// </summary>
        /// <param name="node">The target node this edge transports to.</param>
        /// <param name="azimuth">The azimuth between the source node and the target node.</param>
        public MapEdge(int node, float azimuth)
        {
            this.node = node;
            this.azimuth = azimuth;
        }

        /// <summary>
        /// The azimuth between the source node and the target node.
        /// </summary>
        public float Azimuth => azimuth;

        public override string ToString()
        {
            return $"{nameof(node)}: {node}, {nameof(azimuth)}: {azimuth}";
        }
    }
}