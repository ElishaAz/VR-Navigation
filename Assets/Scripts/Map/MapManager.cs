using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// Manages the map. There should be only one in a scene.
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        public static MapManager instance;

        [SerializeField] [Tooltip("The sphere that will show the current node's image.")]
        private Renderer imageSphere;

        [SerializeField] [Tooltip("A prefab of a non-end Arrow.")]
        private MapArrow arrowPrefab;

        [SerializeField] [Tooltip("A prefab of an end Arrow.")]
        private MapArrow endArrowPrefab;

        /// <summary>
        /// Defines the possible types of cache for the images.
        /// </summary>
        private enum ImageCacheType
        {
            /// <summary>
            /// Load all the resources on start.
            /// </summary>
            LoadOnStart,

            /// <summary>
            /// Preload the resources for a node before it is loaded and free them when the node is too far away.
            /// </summary>
            PreloadCurrent,

            /// <summary>
            /// Load resources on demand and keep them.
            /// </summary>
            OnDemandCache,

            /// <summary>
            /// Load only the needed resources and free them after use.
            /// </summary>
            NoCache
        }

        [SerializeField] [Tooltip("The type of cache to use with the images.")]
        private ImageCacheType imageCache;

        /// <summary>
        /// The resource manager.
        /// </summary>
        private MapResourceManager resourceManager;

        /// <summary>
        /// The map graph.
        /// </summary>
        private MapGraph graph;

        /// <summary>
        /// The last (current) that was loaded.
        /// </summary>
        private MapNode lastNode;

        /// <summary>
        /// A list of the currently existing arrows.
        /// </summary>
        private List<MapArrow> arrows = new List<MapArrow>(); // TODO: reuse arrows.

        private void Awake()
        {
            instance = this;
            resourceManager = new MapResourceManager(Application.persistentDataPath);
        }

        /// <summary>
        /// Initialize the Map.
        /// </summary>
        /// <param name="graph">The Map Graph to initialize to.</param>
        public void Init(MapGraph graph)
        {
            // assign the variables
            this.graph = graph;
            this.graph.resourceManager = resourceManager;

            // call the graph's setup function
            this.graph.SetUp();

            // load resources if needed
            if (imageCache == ImageCacheType.LoadOnStart)
                resourceManager.LoadAllResources(this.graph);
            if (imageCache == ImageCacheType.PreloadCurrent)
                resourceManager.LoadNodeResources(graph.StartPoint);

            // load the initial node
            LoadNode(graph.StartPoint);
        }

        /// <summary>
        /// Load a node from the map.
        /// </summary>
        /// <param name="node">The node to load.</param>
        public void LoadNode(MapNode node)
        {
            Debug.Log("Loading Node: " + node);

            // if the cache mode is set to no cache, we need to free the previous node's resources.
            if (imageCache == ImageCacheType.NoCache && lastNode != default) // TODO: rewrite properly
                resourceManager.FreeNodeResources(lastNode);

            // destroy old arrows
            foreach (var a in arrows)
            {
                if (imageCache == ImageCacheType.PreloadCurrent && a.node != node)
                {
                    // free the resources after a few frames to prevent lagging.
                    StartCoroutine(InvokeLater(() => resourceManager.FreeNodeResources(a.node)));
                }

                Destroy(a.gameObject);
            }

            arrows.Clear();

            // change texture to new node
            imageSphere.material.mainTexture = node.GetTexture;

            // create new arrows
            foreach (var edge in graph.EdgesOf(node))
            {
                CreateArrow(edge);
            }

            lastNode = node;
        }

        /// <summary>
        /// Create a new Arrow.
        /// </summary>
        /// <param name="edge">The edge the new arrow should represent.</param>
        private void CreateArrow(MapEdge edge)
        {
            // create arrow
            MapNode node = graph.NodeOf(edge);
            MapArrow arrow = Instantiate(graph.IsEndPoint(node) ? endArrowPrefab : arrowPrefab);
            arrow.SetUp(node, edge.Azimuth);
            arrows.Add(arrow);

            // load resources if needed.
            if (imageCache == ImageCacheType.PreloadCurrent)
            {
                // load the resources after a few frames to prevent lagging.
                StartCoroutine(InvokeLater(() => resourceManager.LoadNodeResources(node), 2));
            }
        }

        /// <summary>
        /// A coroutine that invokes the given action after a given number of frames.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <param name="frames">The number of frames to wait before invoking the action.</param>
        private IEnumerator InvokeLater(Action action, int frames = 1)
        {
            // while there are frames left
            while (frames > 0)
            {
                // wait for a frame
                yield return null;
                frames--;
            }

            action();
        }
    }
}