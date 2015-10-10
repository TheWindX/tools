using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s_expr
{
    interface Expr
    {
        string tostring();
        void walk(System.Action<Expr> evtPre, System.Action<Expr> evtPost);
    }

    class String_expr : Expr
    {
        public string text;
        public string tostring()
        {
            return text;
        }
        public void walk(System.Action<Expr> evtPre, System.Action<Expr> evtPost)
        {
            if (evtPre != null) evtPre(this);
            if (evtPost != null) evtPost(this);
        }
    }

    class Compound_expr : Expr
    {
        public List<Expr> children = new List<Expr>();
        public string tostring()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('(');
            foreach(var item in children)
            {
                sb.Append(item.tostring());
                sb.Append(' ');
            }
            sb.Append(')');
            return sb.ToString();
        }
        public void walk(System.Action<Expr> evtPre, System.Action<Expr> evtPost)
        {
            if (evtPre != null) evtPre(this);
            foreach(var item in children)
            {
                item.walk(evtPre, evtPost);
            }
            if (evtPost != null) evtPost(this);
        }
    }

    class S_expr_parser
    {
        Compound_expr root = null;

        string source = "";

        public void walk(System.Action<Expr> evtPre, System.Action<Expr> evtPost)
        {
            root.walk(evtPre, evtPost);
        }

        
        public Compound_expr parseCompound(int from, out int idx)
        {
            idx = from;
            parseSpace(from, out idx);
            if (idx == source.Length) return null;
            if (source[idx] != '(') return null;
            idx++;
            var ret = new Compound_expr();
            var oldIdx = idx;
            for(;;)
            {   
                parseSpace(idx, out idx);
                Expr expr = parseCompound(idx, out idx);
                if(expr == null)
                {
                    expr = parseString(idx, out idx);
                    if(expr == null)
                    {
                        break;
                    }
                }
                ret.children.Add(expr);
                if (idx == oldIdx) break;
            }
            if (source[idx] != ')') { idx = from; return null; } 
            idx++;
            return ret;
        }

        public void parseSpace(int from, out int idx)
        {
            idx = from;
            for(;idx != source.Length;idx++)
            {
                if(!char.IsWhiteSpace(source[idx]))return;
            }
        }

        public String_expr parseString(int from, out int idx)
        {
            idx = from;
            for(;idx != source.Length; idx++)
            {
                if(idx == source.Length) return null;
                char ch = source[idx];
                if (char.IsWhiteSpace(ch) || ch==')' || ch=='(')
                {
                    if (from == idx) { return null; }
                    return new String_expr{text = source.Substring(from, idx-from)};
                }
            }
            return null;
        }
        public Expr parse(string src)
        {
            source = src;
            int from = 0;
            int to = 0;
            root = parseCompound(from, out to);
            return root;
        }
    }

    struct Color
    {
        public float a;
        public float r;
        public float g;
        public float b;
    }
    class s_expr
    {
        public static Color toColor(uint argb)
        {
            Color c = new Color();
            c.a = (argb & 0xff000000) >> 24; c.a = c.a/255;
            c.r = (argb & 0xff0000) >> 16; c.r = c.r / 255;
            c.g = (argb & 0xff00) >> 8; c.g = c.g / 255;
            c.b = argb & 0xff; c.b = c.b / 255;
            return c;
        }
        static void Main(string[] args)
        {
            var s = new S_expr_parser();
            s.parse("   (3.24 df asdf ((3 5) () 8) asdf (4) () ) ");
            StringBuilder sb = new StringBuilder();
            
            s.walk(expr =>
                {
                    if(expr is String_expr)
                    {
                        Console.Write(sb.ToString());
                        Console.WriteLine((expr as String_expr).text);
                    }
                    else
                    {
                        Console.Write(sb.ToString());
                        Console.WriteLine('(');
                        sb.Append(' ', 2);
                    }
                }, expr =>
                {
                    if (expr is String_expr)
                    {
                    //    Console.Write(sb.ToString());
                    //    Console.WriteLine((expr as String_expr).text);
                    }
                    else
                    {
                        sb.Remove(sb.Length - 2,2);
                        Console.Write(sb.ToString());
                        Console.WriteLine(')');
                    }
                    
                });
        }

        
    }
}
