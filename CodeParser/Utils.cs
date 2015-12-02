using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeParser.Utils
{
    class StkList<T>
    {
        List<T> mList = new List<T>();

        public void push_back(T o)
        {
            mList.Add(o);
        }

        public void pop_back()
        {
            mList.RemoveAt(mList.Count - 1);
        }

        public List<T> getList()
        {
            return mList;
        }

        public void pop_to(int idx)
        {
            while (mList.Count != idx)
            {
                mList.RemoveAt(mList.Count - 1);
            }
        }

        public T peek()
        {
            return mList[mList.Count - 1];
        }

        public int len()
        {
            return mList.Count;
        }

        public T at(int i)
        {
            return mList[i];
        }

        

        public int rfind(Func<T, bool> pred)
        {
            for(int i = mList.Count-1; i>-1; --i)
            {
                if (pred(mList[i])) return i;
            }
            return -1;
        }

        public int find(Func<T, bool> pred)
        {
            for (int i = 0; i<mList.Count; ++i)
            {
                if (pred(mList[i])) return i;
            }
            return -1;
        }
    }
}
