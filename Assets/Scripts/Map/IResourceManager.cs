using UnityEngine;

namespace Map
{
    /// <summary>
    /// An interface for a class that will manage the resources for a map.
    /// </summary>
    public interface IResourceManager
    {
        Texture2D GetNodeImageTexture(MapNode node);
    }
}