using System.Runtime.Serialization;

namespace VRNavigation.MapData
{
    [DataContract]
    public readonly struct MapNodeText
    {
        /// <summary>
        /// The text to display.
        /// </summary>
        [DataMember] public readonly string text;

        /// <summary>
        /// The time (in seconds after the node is loaded) at which to start displaying the text.
        /// </summary>
        [DataMember] public readonly float startTime;

        /// <summary>
        /// The time (in seconds after the node is loaded) at which to stop displaying the text.
        /// </summary>
        [DataMember] public readonly float endTime;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="startTime">The time at which to start displaying the text.</param>
        /// <param name="endTime">The time at which to stop displaying the text.</param>
        public MapNodeText(string text, float startTime, float endTime)
        {
            this.text = text;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public override string ToString()
        {
            return $"{nameof(text)}: {text}, {nameof(startTime)}: {startTime}, {nameof(endTime)}: {endTime}";
        }
    }
}