using System.Collections;
using System.Collections.Generic;
namespace Logic
{
    public class Point
    {
        public int id { get; set; }
        public string Picture { get; set; }
        public List<Neighbor> Neighbors = new List<Neighbor>();
        public List<Optionaltext> OptionalText = new List<Optionaltext>();
        public override string ToString()
        {
            string s = "ID : " + id + "\nPATH : " + Picture + "\nNeighbors : ";
            foreach (Neighbor n in Neighbors)
            {
                s += n.ToString();
            }
            s += "\nTexts : ";
            foreach (Optionaltext o in OptionalText)
            {
                s += o.ToString();
            }
            return s;
        }
    }
};
