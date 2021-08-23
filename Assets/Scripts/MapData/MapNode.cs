using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace VRNavigation.MapData
{
    /// <summary>
    /// A node in the map, representing a singe location.
    /// </summary>
    [DataContract]
    public readonly struct MapNode
    {
        /// <summary>
        /// The path (relative to the resource location) of the image file of this node.
        /// </summary>
        [DataMember] public readonly string path;

        /// <summary>
        /// Some information about this node.
        /// </summary>
        [DataMember] public readonly MapNodeText[] texts;

        /// <summary>
        /// Create a node.
        /// </summary>
        /// <param name="path">The path (relative to the resource location) of the image file of this node.</param>
        /// <param name="texts"></param>
        [JsonConstructor]
        public MapNode(string path, MapNodeText[] texts)
        {
            this.path = path;
            this.texts = texts;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">The node to copy.</param>
        public MapNode(MapNode other)
        {
            path = other.path;
            texts = other.texts;
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