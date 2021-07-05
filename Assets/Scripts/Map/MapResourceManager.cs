using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// The resource manager for a map.
    /// </summary>
    public class MapResourceManager : IResourceManager
    {
        /// <summary>
        /// The root folder for this map's resources.
        /// </summary>
        private readonly string resourceLocationRoot;

        /// <summary>
        /// A resource entry.
        /// </summary>
        private class Resource
        {
            /// <summary>
            /// The texture.
            /// </summary>
            internal Texture2D tex;

            /// <summary>
            /// A pointer counter.
            /// </summary>
            internal int pointerCounter;

            public Resource(Texture2D tex, int pointerCounter)
            {
                this.tex = tex;
                this.pointerCounter = pointerCounter;
            }
        }

        /// <summary>
        /// The image cache.
        /// </summary>
        private Dictionary<MapNode, Resource> images = new Dictionary<MapNode, Resource>();

        /// <summary>
        /// Create a new resource manager.
        /// </summary>
        /// <param name="resourceLocationRoot">The root folder for this map's resources.</param>
        public MapResourceManager(string resourceLocationRoot)
        {
            this.resourceLocationRoot = resourceLocationRoot;
        }

        /// <summary>
        /// Retrieve the image for a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The image of the node as a Texture2D.</returns>
        public Texture2D GetNodeImageTexture(MapNode node)
        {
            // if the image is not in cache, add it
            if (!images.ContainsKey(node))
                LoadNodeResources(node);

            // return from cache.
            return images[node].tex;
        }

        /// <summary>
        /// Loads the resources for an entire map.
        /// </summary>
        /// <param name="graph">The map graph.</param>
        internal void LoadAllResources(MapGraph graph)
        {
            foreach (var node in graph.GetAllNodes)
            {
                LoadNodeResources(node);
            }
        }

        /// <summary>
        /// Loads the resources for a node.
        /// </summary>
        /// <param name="node">The node.</param>
        internal void LoadNodeResources(MapNode node)
        {
            // if the image is already loaded
            if (images.ContainsKey(node))
            {
                // increase the pointer counter.
                images[node].pointerCounter++;
                return;
            }

            // otherwise - load it
            var tex = IOTools.LoadImage(resourceLocationRoot + Path.DirectorySeparatorChar + node.Path);
            images.Add(node, new Resource(tex, 1));
        }

        /// <summary>
        /// Free the resources of a node to save RAM.
        /// </summary>
        /// <param name="node">The node.</param>
        internal void FreeNodeResources(MapNode node)
        {
            // decrease the pointer counter.
            images[node].pointerCounter--;

            // if the counter reaches 0, free the resource.
            if (images[node].pointerCounter <= 0)
            {
                Object.Destroy(images[node].tex);
                images.Remove(node);
            }
        }
    }
}