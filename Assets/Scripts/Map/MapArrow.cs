using System;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// An Arrow GameObject that is used to travel on the map.
    /// </summary>
    public class MapArrow : MonoBehaviour
    {
        /// <summary>
        /// The target node this arrow will transport to.
        /// </summary>
        internal MapNode node;

        [SerializeField] [Tooltip("The height of the arrow.")]
        private float height = 0;

        [SerializeField] [Tooltip("A multiplier of the x and z of the arrow.")]
        private float distMultiplier = 0.4f;

#if UNITY_EDITOR
        [SerializeField] [Tooltip("Set this to true to trigger the transportation.")]
        private bool trigger;

        private void Update()
        {
            if (trigger)
            {
                trigger = false;
                OnPointerClick();
            }
        }
#endif

        /// <summary>
        /// Set up this arrow with a map node.
        /// </summary>
        /// <param name="node">The target node.</param>
        /// <param name="edgeAzimuth">The azimuth between the source (current) node to the target node.</param>
        public void SetUp(MapNode node, float edgeAzimuth)
        {
            this.node = node;
            // StartCoroutine(node.LoadTexture());

            /* Code copied from old MapBuilder */

            //Calculate Navigation image positions and rotation
            float wantedAzimuthRad = edgeAzimuth * Mathf.PI / 180;
            float x = distMultiplier * Mathf.Cos(-wantedAzimuthRad);
            float y = height;
            float z = distMultiplier * Mathf.Sin(-wantedAzimuthRad);
            transform.localRotation = Quaternion.Euler(0, edgeAzimuth + 90, 0);
            transform.localPosition = new Vector3(x, y, z);
        }

        /// <summary>
        /// Called when this Arrow is gazed at.
        /// </summary>
        private void OnPointerClick()
        {
            MapManager.instance.LoadNode(node);
        }
    }
}