using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class EnumeralbExt
{
    private class FakeEnumerable<T> : IEnumerable<T>
    {
        private IEnumerator<T> m_enumerator;
        public FakeEnumerable(IEnumerator<T> e)
        {
            m_enumerator = e;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_enumerator;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_enumerator;
        }
    }

    public static Tuple<T, IEnumerable<T>> part<T>(this IEnumerable<T> si)
    {
        var iter = si.GetEnumerator();
        bool exist = iter.MoveNext();
        if (exist)
        {
            IEnumerable<T> ret = new FakeEnumerable<T>(iter);
            return Tuple.make(iter.Current, ret);
        }
        return null;
    }
}

namespace ns_parser
{

    public class Expr
    {
        public string tag = "";
        public int from = 0;
        public int to = 0;
        public List<Expr> children = new List<Expr>();
        public void push(Expr e)
        {
            children.Add(e);
        }

        public void pop(Expr e)
        {
            children.RemoveAt(children.Count-1);
        }
        
        public Expr clone()
        {
            var e = new Expr();
            e.tag = tag;
            e.from = from;
            e.to = to;
            foreach(var sube in children)
            {
                e.children.Add(sube);
            }
            return e;
        }

        public string toString(string source)
        {
            return source.Substring(from, to - from);
        }

        public string toXML(string source, int space)
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append(new string(' ', space));
            sb.Append("<");
            sb.Append(tag);
            sb.Append(">\n");

            if (tag == "tok" || tag == "string")
            {
                sb.Append(new string(' ', space));
                sb.Append( toString(source));
                sb.Append('\n');
            }
            else
            {
                foreach (var c in children)
                {
                    sb.Append(c.toXML(source, space + 1));
                }
            }
            

            sb.Append(new string(' ', space));
            sb.Append("</");
            sb.Append(tag);
            sb.Append(">\n");
            return sb.ToString();
        }
    }


    class Parser2
    {
        public void init(string src)
        {
            source = src;
            idx = 0;
        }

        public string source;
        int idx = 0;

        public Func<IEnumerable<Expr>> tok(string str)
        {
            return () =>
            {
                return _tok(str);
            };
        }

        IEnumerable<Expr> _tok(string str)
        {
            var oldIdx = idx;
            skipSpace();
            var from = idx;
            if (idx == source.Length) 
            {
                idx = oldIdx;
                yield break;
            }
            for(int i = 0; i<str.Length; ++i)
            {
                if (idx == source.Length) 
                {
                    idx = oldIdx;
                    yield break;
                }
                if(source[idx++] != str[i])
                {
                    idx = oldIdx;
                    yield break;
                }
            }
            var e = new Expr();
            e.tag = "tok";
            e.from = from;
            e.to = idx;
            skipSpace();
            yield return e;
        }

        public Func<IEnumerable<Expr>> str()
        {
            return () =>
            {
                return _str();
            };
        }
        public IEnumerable<Expr> _str()
        {
            var oldIdx = idx;
            skipSpace();
            if (idx == source.Length)
            {
                idx = oldIdx;
                yield break;
            }
            var from = idx;
            var e = new Expr();
            e.tag = "string";
            e.from = from;

            for (; idx != source.Length; idx++)
            {
                var c = source[idx];
                if (!char.IsLetterOrDigit(c))
                {
                    break;
                }
            }
            e.to = idx;
            if(e.from == e.to)
            {
                idx = oldIdx;
                yield break;
            }
            skipSpace();
            yield return e;
        }

        void skipSpace()
        {
            for (; idx != source.Length;idx++ )
            {
                if(!char.IsWhiteSpace(source[idx]) )
                {
                    break;
                }
            }
        }

        public IEnumerable<Expr> parseAndHelper(Expr p, IEnumerable<Func<IEnumerable<Expr>>> sublexp)
        {
            var from = idx;
            var ad = sublexp.part();
            if(ad == null)
            {
                idx = from;
                yield return p;
                yield break;
            }

            foreach(var e in ad._0())
            {
                var p1 = p.clone();    
                p1.push(e);
                var es = parseAndHelper(p1.clone(), ad._1);
                foreach(var e1 in es)
                {
                    e1.to = idx;
                    yield return e1;
                }
                idx = from;
            }
        }

        public Func<IEnumerable<Expr>> and(string tag, params Func<IEnumerable<Expr>>[] sublexp)
        {
            return () =>
            {
                return _and(tag, sublexp);
            };
        }
        public IEnumerable<Expr> _and(string tag, Func<IEnumerable<Expr>>[] sublexp)
        {
            var from = idx;
            Expr e = new Expr();
            e.tag = tag;
            e.from = from;

            var es = parseAndHelper(e, sublexp);
            bool empty = true;
            foreach (var e1 in es)
            {
                empty = false;
                yield return e1;
            }
        }

        void loopHelper(Expr e, Func<IEnumerable<Expr>> lexp)
        {
            var from = idx;
            var p = lexp().part();
            if(p != null)
            {
                e.push(p._0);
                loopHelper(e, lexp);
            }
        }

        public Func<IEnumerable<Expr>> loop(string tag, Func<IEnumerable<Expr>> lexp)
        {
            return () =>
                {
                    return _loop(tag, lexp);
                };
        }

        public IEnumerable<Expr> _loop(string tag, Func<IEnumerable<Expr>> lexp)
        {
            var from = idx;
            Expr e = new Expr();
            e.tag = tag;
            e.from = from;
            loopHelper(e, lexp);
            if(e.children.Count == 0)
            {
                idx = from;
                yield break;
            }
            yield return e;
        }

        public Func<IEnumerable<Expr>> or(string tag, params Func<IEnumerable<Expr>>[] sublexp)
        {
            return () =>
            {
                return _or(tag, sublexp);
            };
        }

        public IEnumerable<Expr> _or(string tag, Func<IEnumerable<Expr>>[] sublexp)
        {
            var from = idx;
            Expr e = new Expr();
            e.tag = tag;
            e.from = from;

            foreach(var es in sublexp)
            {
                foreach(var e1 in es())
                {
                    var ee = e.clone();
                    ee.push(e1);
                    yield return ee;
                    idx = from;
                }
            }
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            Parser2 v = new Parser2();

            Func<IEnumerable<Expr>> item = null;
            Func<IEnumerable<Expr>> item_list = null;
            Func<IEnumerable<Expr>> group = null;
            Func<IEnumerable<Expr>> lp = null;
            Func<IEnumerable<Expr>> rp = null;

            item = v.or("item", v.str(), ()=>group());
            item_list = v.loop("item_list", item);
            group = v.or("group", v.and("group", v.tok("("), item_list, v.tok(")"));

            v.init("( (as 34) df tt)");

            foreach (var e in group())
            {
                Console.Write(e.toXML(v.source, 0));
            }
            
        }
    }
}
