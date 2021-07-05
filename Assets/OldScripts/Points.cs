using System.Collections;
using System.Collections.Generic;
namespace Logic
{
    public class Points
    {
        public string Projectname { get; set; }
        public string NavigationImage { get; set; }
        public string FinalNavigationImage { get; set; }
        public long StartPoint { get; set; }
        public List<long> EndPoints = new List<long>();
        public List<Point> points = new List<Point>();
        public override string ToString()
        {
            string s = "";
            s += "Project name : " + Projectname;
            s += ",\nTrans img : " + NavigationImage;
            s += ",\nFinal trans img : " + FinalNavigationImage + ",\n";
            s += ",\nStart Point : " + StartPoint + ",\n";
            s += ",\nEnd Points : " + string.Join(",", EndPoints) + ",\n";
            foreach (Point p in points)
            {
                s += p.ToString() + "\n";
            }
            return s;
        }
    }

};
