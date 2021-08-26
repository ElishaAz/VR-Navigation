using System;
using System.Collections;
using System.Collections.Generic;
using Log;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// Manages the map. There should be only one in a scene.
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        [SerializeField] [Tooltip("The sphere that will show the current node's image.")]
        private Renderer imageSphere;

        [SerializeField] [Tooltip("A prefab of a non-end Arrow.")]
        private MapArrow arrowPrefab;

        [SerializeField] [Tooltip("A prefab of an end Arrow.")]
        private MapArrow endArrowPrefab;

        [SerializeField] [Range(1, 100)] private float actionsPerSecond = 10;

        private InvokeLater invokeLater;

        private Action onNodeLoad;

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
            /// Load the resources for the arrow that is currently gazed at and free them when the node is too far away.
            /// </summary>
            LoadOnHover,

            /// <summary>
            /// Load the resources for the arrow that is currently gazed and do not free them.
            /// </summary>
            LoadOnHoverKeep,

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
        private MapGraphWrapped graph;

        /// <summary>
        /// The last (current) node that was loaded.
        /// </summary>
        private MapNodeWrapper lastNode;

        /// <summary>
        /// A list of the currently existing arrows.
        /// </summary>
        private List<MapArrow> arrows = new List<MapArrow>(); // TODO: reuse arrows.

        private void Awake()
        {
            Instance = this;

            invokeLater = new InvokeLater(actionsPerSecond);
            StartCoroutine(invokeLater.MainCoroutine());
        }

        /// <summary>
        /// Initialize the Map.
        /// </summary>
        /// <param name="graph">The Map Graph to initialize to.</param>
        /// <param name="resourceLocationRoot">The path to the root of the resources.</param>
        public void Init(MapGraphWrapped graph, string resourceLocationRoot)
        {
            // assign the variables
            resourceManager = new MapResourceManager(resourceLocationRoot);
            this.graph = graph;
            this.graph.resourceManager = resourceManager;

            // call the graph's setup function
            this.graph.SetUp();

            // load resources if needed
            if (imageCache == ImageCacheType.LoadOnStart)
            {
            }

            var startPoint = graph.StartPoint;

            if (imageCache == ImageCacheType.PreloadCurrent)
                StartCoroutine(startPoint.LoadResources());

            // load the initial node
            LoadNode(startPoint);
        }

        /// <summary>
        /// Load a node from the map.
        /// </summary>
        /// <param name="node">The node to load.</param>
        public void LoadNode(MapNodeWrapper node)
        {
            Debug.Log("Loading Node: " + node);

            // if the cache mode is set to no cache, we need to free the previous node's resources.
            if (imageCache == ImageCacheType.NoCache && lastNode != default) // TODO: rewrite properly
                lastNode.FreeResources();

            // destroy old arrows
            foreach (var a in arrows)
            {
                if ((imageCache == ImageCacheType.PreloadCurrent || imageCache == ImageCacheType.LoadOnHover) &&
                    a.node != node)
                {
                    // free the resources after a few frames to prevent lagging.
                    // StartCoroutine(invokeLater.AddNextFrame(() => resourceManager.FreeNodeResources(a.node)));
                    node.FreeResources(imageCache == ImageCacheType.LoadOnHover);
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
            onNodeLoad?.Invoke();
        }

        /// <summary>
        /// Create a new Arrow.
        /// </summary>
        /// <param name="edge">The edge the new arrow should represent.</param>
        private void CreateArrow(MapEdgeWrapper edge)
        {
            // create arrow
            MapNodeWrapper node = graph.NodeOf(edge);
            MapArrow arrow = Instantiate(graph.IsEndPoint(node) ? endArrowPrefab : arrowPrefab);

            if (imageCache == ImageCacheType.LoadOnHover || imageCache == ImageCacheType.LoadOnHoverKeep)
                arrow.SetUp(node, edge.Azimuth, resourceManager);
            else
                arrow.SetUp(node, edge.Azimuth, null);

            arrows.Add(arrow);

            // load resources if needed.
            if (imageCache == ImageCacheType.PreloadCurrent)
            {
                // load the resources after a few frames to prevent lagging.
                StartCoroutine(invokeLater.AddNextFrame(() =>
                    StartCoroutine(node.LoadResources())));
            }
        }

        /// <summary>
        /// The currently loaded node.
        /// </summary>
        public MapNodeWrapper CurrentNode => lastNode;

        /// <summary>
        /// Add an action to invoke on node load.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        public void AddOnNodeLoad(Action action)
        {
            onNodeLoad += action;
        }
    }
}