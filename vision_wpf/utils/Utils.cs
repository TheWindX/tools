using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_vision.ns_utils
{
    public class ListAdvance<T>
    {
        List<T> _vals = new List<T>();
        public IEnumerable<T> vals
        {
            get
            {
                return _vals;
            }
        }

        public bool add(T t)
        {
            var idx = _vals.IndexOf(t);
            if (idx != -1) return false;
            _vals.Add(t);
            return true;
        }

        public bool remove(T t)
        {
            var idx = _vals.IndexOf(t);
            if (idx != -1) return false;
            _vals.Remove(t);
            return true;
        }

        public void clear()
        {
            _vals.Clear();
        }

        public void tailer(T t)
        {
            var idx = _vals.IndexOf(t);
            if (idx == -1) throw new Exception("tailer error");
            if (idx + 1 >= _vals.Count) return;

            _vals.RemoveAt(idx);
            _vals.Insert(idx + 1, t);
        }

        public void header(T t)
        {
            var idx = _vals.IndexOf(t);
            if (idx == -1) throw new Exception("tailer error");
            if (idx < 1) return;
            _vals.RemoveAt(idx);
            _vals.Insert(idx - 1, t);
        }

        public void insert_before(T b, T t)
        {
            var idx = _vals.IndexOf(t);
            
            var idx1 = _vals.IndexOf(b);
            if (idx1 == -1) throw new Exception("insert_before error");

            if(idx != -1)
            {
                _vals.RemoveAt(idx);
            }

            var idx2 = _vals.IndexOf(b);
            _vals.Insert(idx2, t);
        }

        public void insert_after(T a, T t)
        {
            var idx = _vals.IndexOf(t);

            var idx1 = _vals.IndexOf(a);
            if (idx1 == -1) throw new Exception("insert_after error");

            if (idx != -1)
            {
                _vals.RemoveAt(idx);
            }

            var idx2 = _vals.IndexOf(a);
            _vals.Insert(idx2+1, t);
        }
    }
}
