using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;

namespace VRNavigation.MapData
{
    /// <summary>
    /// A node in the map, representing a singe location.
    /// </summary>
    [DataContract]
    public struct MapNode
    {
        /// <summary>
        /// The path (relative to the resource location) of the image file of this node.
        /// </summary>
        [DataMember] public readonly string path;

        // TODO: optional text

        /// <summary>
        /// Create a node.
        /// </summary>
        /// <param name="path">The path (relative to the resource location) of the image file of this node.</param>
        public MapNode(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">The node to copy.</param>
        public MapNode(MapNode other)
        {
            path = other.path;
        }

        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>A string representing this object.</returns>
        public override string ToString()
        {
            return $"{nameof(path)}: {path}";
        }

        #region operators

        public bool Equals(MapNode other)
        {
            return path == other.path;
        }

        public override bool Equals(object obj)
        {
            return obj is MapNode other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (path != null ? path.GetHashCode() : 0);
        }

        public static bool operator ==(MapNode left, MapNode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MapNode left, MapNode right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}