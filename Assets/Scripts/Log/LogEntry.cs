namespace Log
{
    /// <summary>
    /// A log entry.
    /// </summary>
    public readonly struct LogEntry
    {
        /// <summary>
        /// The time in seconds of this entry.
        /// </summary>
        public readonly float time;

        /// <summary>
        /// The current node's ID.
        /// </summary>
        public readonly int node;

        /// <summary>
        /// The player's current orientation.
        /// </summary>
        public readonly float orientation;

        /// <summary>
        /// Is this entry logged because of a node change?
        /// </summary>
        public readonly bool nodeChange;

        /// <summary>
        /// Create a new log entry.
        /// </summary>
        /// <param name="time">The time in seconds of this entry.</param>
        /// <param name="node">The current node's ID.</param>
        /// <param name="orientation">The player's current orientation.</param>
        /// <param name="nodeChange">Is this entry logged because of a node change?</param>
        public LogEntry(float time, int node, float orientation, bool nodeChange)
        {
            this.time = time;
            this.node = node;
            this.orientation = orientation;
            this.nodeChange = nodeChange;
        }

        /// <summary>
        /// Creates a CSV string of this entry.
        /// </summary>
        /// <returns>A CSV representation of this entry.</returns>
        public string ToCSV()
        {
            return $"{time}, {node}, {orientation}, {nodeChange}";
        }

        /// <summary>
        /// Creates the CSV header.
        /// </summary>
        /// <returns>A CSV header for use with <see cref="ToCSV"/>.</returns>
        public static string CSVHeader()
        {
            return $"{nameof(time)}, {nameof(node)}, {nameof(orientation)}, {nameof(nodeChange)}";
        }
    }
}