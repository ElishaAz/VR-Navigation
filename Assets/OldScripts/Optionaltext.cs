using System.Collections;
using System.Collections.Generic;
namespace Logic
{
    public class Optionaltext
    {
        public string text { get; set; }
        public float DurationInSeconds { get; set; }
        public float whenToDisplay { get; set; }
        public override string ToString()
        {
            string s = "[Text : " + text + " , Duration : " + DurationInSeconds + " , after : " + whenToDisplay + "]";
            return s;
        }
    }
};