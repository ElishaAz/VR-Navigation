using System.Runtime.Serialization;

namespace VRNavigation.MapData
{
    /// <summary>
    /// An edge in the map, representing a path from one location to another (not bidirectional).
    /// </summary>
    [DataContract]
    public struct MapEdge
    {
        /// <summary>
        /// The destination node's id.
        /// </summary>
        [DataMember] public readonly int node;

        /// <summary>
        /// The azimuth between the source node and the destination node.
        /// </summary>
        [DataMember] public readonly float azimuth;

        /// <summary>
        /// Create a new map edge.
        /// </summary>
        /// <param name="node">The destination node's id.</param>
        /// <param name="azimuth">The azimuth between the source node and the destination node.</param>
        public MapEdge(int node, float azimuth)
        {
            this.node = node;
            this.azimuth = azimuth;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">The edge to copy.</param>
        public MapEdge(MapEdge other)
        {
            this.node = other.node;
            this.azimuth = other.azimuth;
        }

        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>A string representing this object.</returns>
        public override string ToString()
        {
            return $"{nameof(node)}: {node}, {nameof(azimuth)}: {azimuth}";
        }

        #region operators

        public bool Equals(MapEdge other)
        {
            return node == other.node && azimuth.Equals(other.azimuth);
        }

        public override bool Equals(object obj)
        {
            return obj is MapEdge other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (node * 397) ^ azimuth.GetHashCode();
            }
        }

        public static bool operator ==(MapEdge left, MapEdge right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MapEdge left, MapEdge right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}