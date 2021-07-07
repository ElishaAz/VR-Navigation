using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using VRNavigation.MapData;

namespace Map
{
    /// <summary>
    /// A node in the map, representing a singe location.
    /// </summary>
    public struct MapNodeWrapper
    {
        /// <summary>
        /// The node we're wrapping.
        /// </summary>
        private readonly MapNode node;

        /// <summary>
        /// The ID of this node in the graph.
        /// </summary>
        private readonly int id;

        /// <summary>
        /// The graph that this node belongs to. This is set by MapGraph before the node is given to the user.
        /// </summary>
        private readonly MapGraphWrapped parentGraph;

        /// <summary>
        /// The resource manager for this map. This is set by MapGraph before the node is given to the user.
        /// </summary>
        internal readonly MapResourceManager resourceManager;

        // TODO: optional text

        /// <summary>
        /// Create a node.
        /// </summary>
        /// <param name="node">The node to wrap.</param>
        /// <param name="id">The id of the node in the graph.</param>
        /// <param name="parentGraph">The graph containing this node.</param>
        /// <param name="resourceManager">The resource manager for this node.</param>
        public MapNodeWrapper(MapNode node, int id, MapGraphWrapped parentGraph, MapResourceManager resourceManager)
        {
            this.id = id;
            this.node = node;
            this.parentGraph = parentGraph;
            this.resourceManager = resourceManager;
        }

        /// <summary>
        /// Retrieves the image of this node as a texture.
        /// </summary>
        public Texture2D GetTexture =>
            resourceManager?.GetNodeImageTexture(node);

        /// <summary>
        /// The path (relative to the resource location) of the image file of this node.
        /// </summary>
        internal string Path => node.path;

        /// <summary>
        /// The ID of this node in the graph.
        /// </summary>
        internal int ID => id;

        public IEnumerator LoadResources()
        {
            yield return resourceManager.LoadNodeResources(node);
        }

        public void FreeResources(bool ignoreNonExisting = false)
        {
            resourceManager.FreeNodeResources(node, ignoreNonExisting);
        }

        public override string ToString()
        {
            return $"{nameof(node)}: {node}, {nameof(id)}: {id}";
        }

        #region opperators

        public bool Equals(MapNodeWrapper other)
        {
            return node.Equals(other.node) && id == other.id;
        }

        public override bool Equals(object obj)
        {
            return obj is MapNodeWrapper other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (node.GetHashCode() * 397) ^ id;
            }
        }

        public static bool operator ==(MapNodeWrapper left, MapNodeWrapper right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MapNodeWrapper left, MapNodeWrapper right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}