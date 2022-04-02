using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab7
{
    class CreateShape
    {
        public PPoint create(StreamReader stream)
        {
            string s = stream.ReadLine();
            PPoint p=null;
            switch(s)
            {
                case "Circle":
                    p = new Circle(0, 0, 0);
                    p.Load(stream);
                    break;
                case "Triangle":
                    p = new Triangle(0,0,0);
                    p.Load(stream);
                    break;
                case "Square":
                    p = new Square(0, 0, 0);
                    p.Load(stream);
                    break;
                case "Line":
                    p = new Line(0, 0, 0);
                    p.Load(stream);
                    break;
            }
            return p;
        }
    }
}
