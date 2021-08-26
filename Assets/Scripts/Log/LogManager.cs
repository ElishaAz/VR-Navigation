using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Map;
using UnityEngine;
using UnityEngine.SpatialTracking;

namespace Log
{
    /// <summary>
    /// Manages the logging.
    /// </summary>
    public class LogManager : MonoBehaviour
    {
        /// <summary>
        /// A list of all the log entries.
        /// </summary>
        private readonly List<LogEntry> entries = new List<LogEntry>();

        /// <summary>
        /// The time at which the log manager was started.
        /// </summary>
        private float startTime;

        /// <summary>
        /// The log writer.
        /// </summary>
        private LogWriter logWriter;

        [SerializeField] [Tooltip("The time in seconds between timed log entries.")]
        private float logTime = 0.5f;

        [SerializeField] [Tooltip("The player's TrackedPoseDriver.")]
        private TrackedPoseDriver player;

        [SerializeField] [Tooltip("The map manager.")]
        private MapManager mapManager;

        private void Awake()
        {
            startTime = Time.time;

            logWriter = new LogWriter(Path.Combine(GlobalVars.LogLocation,
                "vrn_log-" + DateTime.Now.ToString("yyyy-M-dd_HH-mm-ss") + ".csv"));
            logWriter.Log(LogEntry.CSVHeader());

            MapManager.Instance.AddOnNodeLoad(() => Log(true));

            StartCoroutine(LogCoroutine());
        }

        /// <summary>
        /// Log an entry.
        /// </summary>
        /// <param name="nodeChange">Is this entry logged because of a node change?</param>
        private void Log(bool nodeChange)
        {
            float azimuth = (player.transform.localEulerAngles.y + 270) % 360;
            if (azimuth > 180)
            {
                azimuth -= 360;
            }

            var entry = new LogEntry(Time.time - startTime, mapManager.CurrentNode.ID,
                azimuth, nodeChange);
            entries.Add(entry);
            logWriter.Log(entry.ToCSV());
        }

        /// <summary>
        /// The logging coroutine.
        /// </summary>
        private IEnumerator LogCoroutine()
        {
            var wait = new WaitForSeconds(logTime);
            while (true)
            {
                yield return wait;
                Log(false);
            }
        }
    }
}