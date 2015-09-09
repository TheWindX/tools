using System;
using System.Collections;
using System.Collections.Generic;

public class ListAdvance<T> : IEnumerable<T>
{
    public ListAdvance()
    {

    }
    List<T> mItems = new List<T>();

    public void check(T item)
    {
        if (mItems.Contains(item))
        {
            throw new Exception("mItems already has item");
        }
    }

    public void Add(T item)
    {
        check(item);
        mItems.Add(item);
    }

    public void Remove(T item)
    {
        check(item);
        mItems.Remove(item);
    }

    public int indexOf(T item)
    {
        for(int i = 0; i<mItems.Count; ++i)
        {
            if(mItems[i].Equals(item) )
            {
                return i;
            }
        }
        return -1;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return mItems.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return mItems.GetEnumerator();
    }
    
}