using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace VRNavigation.MapData
{
    [DataContract]
    public readonly struct MapInfo
    {
        /// <summary>
        /// The name of the map.
        /// </summary>
        [DataMember] public readonly string name;

        /// <summary>
        /// The version number of the map.
        /// </summary>
        [DataMember] public readonly float version;

        /// <summary>
        /// Map info constructor.
        /// </summary>
        /// <param name="name">The name of the map.</param>
        /// <param name="version">The version number of the map.</param>
        [JsonConstructor]
        public MapInfo(string name, float version)
        {
            this.name = name;
            this.version = version;
        }

        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>A string representing this object.</returns>
        public override string ToString()
        {
            return $"{nameof(name)}: {name}, {nameof(version)}: {version}";
        }

        #region operators

        public bool Equals(MapInfo other)
        {
            return name == other.name && version.Equals(other.version);
        }

        public override bool Equals(object obj)
        {
            return obj is MapInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0) * 397) ^ version.GetHashCode();
            }
        }

        public static bool operator ==(MapInfo left, MapInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MapInfo left, MapInfo right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}