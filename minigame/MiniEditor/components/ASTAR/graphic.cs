using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    class Graphic
    {
        public class Vertext
        {
            internal double x_;
            internal double y_;
            public int id;
            public override string ToString()
            {
                return "(" + x_.ToString() + "," + y_.ToString() + ")";
            }

            public Vertext(double x, double y)
            {
                x_ = x;
                y_ = y;
            }

            public static Vertext operator +(Vertext v1, Vertext v2)
            {
                return new Vertext(v1.x_ + v2.x_, v1.y_ + v2.y_);
            }

            public static Vertext operator -(Vertext v1, Vertext v2)
            {
                return new Vertext(v1.x_ - v2.x_, v1.y_ - v2.y_);
            }

            public double magnitude()
            {
                return Math.Sqrt(x_ * x_ + y_ * y_);
            }

            public static double distance(Vertext v1, Vertext v2)
            {
                return (v1 - v2).magnitude();
            }
        }
        List<Vertext> mVertext = new List<Vertext>();

        public class Edge
        {
            public Vertext v1;
            public Vertext v2;
            public double length
            {
                get
                {
                    //优化
                    return Vertext.distance(v1, v2);
                }
            }
        }
        List<Edge> mEdges = new List<Edge>();
        public Vertext genVertex(double x, double y)
        {
            var v = new Vertext(x, y);
            v.id = mVertext.Count;
            mVertext.Add(v);
            return v;
        }

        public Edge genEdge(Vertext v1, Vertext v2)
        {
            return new Edge { v1 = v1, v2 = v2 };
        }

        public Vertext getVertex(int id)
        {
            return mVertext[id];
        }


    }


}
