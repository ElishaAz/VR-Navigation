using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

namespace Map
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
        [DataMember] private string path;

        /// <summary>
        /// The ID of this node in the graph.
        /// </summary>
        internal int id;

        /// <summary>
        /// The graph that this node belongs to. This is set by MapGraph before the node is given to the user.
        /// </summary>
        internal MapGraph parentGraph;

        /// <summary>
        /// The resource manager for this map. This is set by MapGraph before the node is given to the user.
        /// </summary>
        internal IResourceManager resourceManager;

        // TODO: optional text

        /// <summary>
        /// Create a node.
        /// </summary>
        /// <param name="path"></param>
        public MapNode(string path)
        {
            id = -1;
            this.path = path;
            parentGraph = null;
            resourceManager = null;
        }

        /// <summary>
        /// Retrieves the image of this node as a texture.
        /// </summary>
        public Texture2D GetTexture =>
            resourceManager?.GetNodeImageTexture(this);
        
        /// <summary>
        /// The path (relative to the resource location) of the image file of this node.
        /// </summary>
        internal string Path => path;

        public override string ToString()
        {
            return $"{nameof(path)}: {path}, {nameof(id)}: {id}";
        }


        #region opperators

        public bool Equals(MapNode other)
        {
            return path == other.path && id == other.id;
        }

        public override bool Equals(object obj)
        {
            return obj is MapNode other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((path != null ? path.GetHashCode() : 0) * 397) ^ id;
            }
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