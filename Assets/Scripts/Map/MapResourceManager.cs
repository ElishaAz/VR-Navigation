using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using VRNavigation.MapData;

namespace Map
{
    /// <summary>
    /// The resource manager for a map.
    /// </summary>
    public class MapResourceManager
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
            {
                // load resources synchronously
                var tex = IOTools.LoadImage(Path.Combine(resourceLocationRoot, node.path));
                images.Add(node, new Resource(tex, 1));
            }

            // return from cache.
            return images[node].tex;
        }

        /// <summary>
        /// Loads the resources for a node.
        /// </summary>
        /// <param name="node">The node.</param>
        internal IEnumerator LoadNodeResources(MapNode node)
        {
            Debug.Log($@"Loading {node} resources");

            // if the image is already loaded
            if (images.ContainsKey(node))
            {
                // increase the pointer counter.
                images[node].pointerCounter++;
                yield break;
            }

            var path = "file://" + Path.Combine(resourceLocationRoot, node.path);

            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    // if the image was already loaded in the meantime
                    if (images.ContainsKey(node))
                    {
                        yield break;
                    }

                    images.Add(node, new Resource(DownloadHandlerTexture.GetContent(uwr), 1));
                }
            }


            // // otherwise - load it
            // var tex = IOTools.LoadImage(resourceLocationRoot + Path.DirectorySeparatorChar + node.Path);
            // images.Add(node, new Resource(tex, 1));
        }

        /// <summary>
        /// Free the resources of a node to save RAM.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="ignoreNonExisting"></param>
        internal void FreeNodeResources(MapNode node, bool ignoreNonExisting = false)
        {
            if (!images.ContainsKey(node))
            {
                if (!ignoreNonExisting)
                {
                    Debug.LogError($@"Node {node} is being freed before loading.");
                }

                return;
            }

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