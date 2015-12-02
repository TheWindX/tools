using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeParser
{
    class stkNode<T>
    {
        public stkNode<T> parent = null;
        public T val;
        public static stkNode<T> create(T v)
        {
            var r = new stkNode<T>();
            r.val = v;
            return r;
        }

        public stkNode<T> push(T v)
        {
            var r = stkNode<T>.create(v);
            r.parent = this;
            return r;
        }

        public stkNode<T> pop()
        {
            return parent;
        }
    }


    class CodeSource
    {
        private string mSource = "";
        private int idx = 0;
        private int marked = -1;

        private int len = 0;

        stkNode<int> posStk = new stkNode<int>();


        public void setSoruce(string src)
        {
            mSource = src;
            idx = 0;
            marked = 0;
            len = src.Length;
            posStk = stkNode<int>.create(idx);
        }

        public char getchar()
        {
            return mSource[idx++];
        }

        public bool matchString(string s)
        {
            string dst = s;
            int di = 0;
            string src = mSource;
            int si = idx;

            for (; di != dst.Length; ++di)
            {
                if (si == len) return false;
                if(src[si++] != dst[si++])
                {
                    return false;
                }
            }

            idx = si;
            return true;
        }

        public void skipSpace()
        {
            while(idx != len)
            {
                if (!char.IsWhiteSpace(mSource[idx])) return;
                idx++;
            }
        }

        public bool eof()
        {
            return idx == len;
        }

        public void pushPos()
        {
            posStk = posStk.push(idx);
        }

        public void popPos()
        {
            posStk = posStk.pop();
            idx = posStk.val;
        }

        public void markPos()
        {
            marked = idx;
        }

        private void getLocationMarked(out int row, out int col)
        {
            int l = 1;
            int c = 1;
            for (int i = 0; i < marked; ++i)
            {
                if (mSource[i] == '\n')
                {
                    l++;
                    c = 0;
                }
                c++;
            }
            row = l;
            col = c;
        }

        public string getLineMarked()
        {
            int l = 1;
            int c = 1;
            int lbegin = 0;
            for (int i = 0; i < marked; ++i)
            {
                if (mSource[i] == '\n')
                {
                    lbegin = i + 1;
                    l++;
                    c = 0;
                }
                c++;
            }
            int lend = 0;
            for (int i = marked; i<len; ++i)
            {
                if (mSource[i] == '\n')
                {
                    lend = i;
                    break;
                }
            }

            var r = mSource.Substring(lbegin, marked - lbegin) + "^^^" + mSource.Substring(marked, lend - marked);
            r = r.Replace("\r", "").Replace("\n", "\\n");
            return r;
        }

    }
}
