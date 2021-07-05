using System.Collections;
using System.Collections.Generic;
namespace Logic
{
    public class Neighbor
    {
        public int PointID { get; set; }
        public float Azimut { get; set; }
        public override string ToString()
        {
            string s = "[ID : " + PointID + " , Azimuth : " + Azimut + "]";
            return s;
        }
    }
};