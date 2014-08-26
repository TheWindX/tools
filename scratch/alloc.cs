using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System;


/// <summary>
/// BLOCK
/// (LEN, PRE, NEXT, VALID,.....IDX) = IDX+4+LEN+1 //valid = v0, use1, nv2;
/// </summary>
class Allocator
{
    private UInt32[] mPool = null;

    private UInt32 mSizeAlloc = 0;
    private UInt32 sizeAlloc
    {
        get { return mSizeAlloc; }
        set
        {
            mSizeAlloc = value;
            if (mSizeAlloc == 0)
            {
                Console.WriteLine("reset!!!");
                reset();
            }
        }
    }

    private void reset()
    {
        var len = (UInt32)mPool.Length;
        mHeadIdx = 0;
        mHeadEnd = len - 1;

        //建立链表
        mPool[mHeadIdx] = len - 5;
        mPool[mHeadIdx + 1] = 0;
        mPool[mHeadIdx + 2] = 0;
        mPool[mHeadIdx + 3] = 0;//v0
        mPool[len - 1] = 0;
    }


    public UInt32 getLen(UInt32 ptr)
    {
        return mPool[ptr - 4];
    }

    private UInt32 mHeadIdx = 0;
    private UInt32 mHeadEnd = 0;
    public void init(int times)
    {
        UInt32 len = (UInt32)Math.Pow(2, times);
        mPool = new UInt32[len];
        
        mHeadIdx = 0;
        mHeadEnd = len - 1;

        //建立链表
        mPool[mHeadIdx] = len-5;
        mPool[mHeadIdx + 1] = 0;
        mPool[mHeadIdx + 2] = 0;
        mPool[mHeadIdx + 3] = 0;//v0
        mPool[len - 1] = 0;
    }

    private bool allFilled = false;
    private UInt32 getEnoughSpace(UInt32 curIdx, UInt32 len)
    {
        UInt32 curLen = mPool[curIdx];
        if (curLen > len + 5)
        {
            return curIdx;
        }
        else if (curLen == len + 5) //use
        {
            allFilled = true;
            return curIdx;
        }
        else
        {
            var pre = mPool[curIdx + 1];
            return pre == mHeadIdx ? UInt32.MaxValue : getEnoughSpace(pre, len);
        }
    }

    public UInt32 alloc(UInt32 len)
    {
        UInt32 curIdx = mHeadIdx;
        UInt32 curLen = mPool[curIdx];

        allFilled = false;
        var newIdx = getEnoughSpace(mHeadIdx, len);
        mHeadIdx = newIdx;
        
        if (allFilled)
        {
            var pre = mPool[1];
            var next = mPool[2];
            if (pre == curIdx) throw new Exception("use all memory");

            mPool[pre + 2] = next;
            mPool[next + 1] = pre;
            mPool[curIdx + 3] = 1;//use1
            mHeadIdx = pre;
            sizeAlloc = sizeAlloc + len;
            return curIdx+4;
        }
        else
        {
            var left = (curLen - len - 5);
            newIdx = curIdx + 4 + left;

            mPool[curIdx] = left;
            mPool[curIdx + left + 4] = curIdx;

            mPool[newIdx] = len;
            mPool[newIdx + 3] = 1;//use1
            mPool[newIdx + len + 4] = newIdx;
            sizeAlloc = sizeAlloc + len;
            return newIdx+4;
        }
    }

    public void free(UInt32 m)
    {
        var idx = m - 4;
        var len = mPool[idx];
        sizeAlloc = sizeAlloc - len;
        var pre = mPool[idx + 1];
        var next = mPool[idx + 2];

        var hpre = mPool[mHeadIdx + 1];
        var hnext = mPool[mHeadIdx + 2];

        mPool[hpre + 2] = idx;
        mPool[hnext + 1] = idx;

        mPool[idx + 1] = hpre;
        mPool[idx + 2] = hnext;
        mPool[idx + 3] = 0;//v0
    }


    public static void test1()
    {
        Allocator allo = new Allocator();
        allo.init(24);

        var ptrs = new HashSet<UInt32>();


        Random r = new Random();

        for (int l = 0; l < 1000; ++l)
        {
            for (int i = 0; i < 20; i++)
            {
                var rlen = (UInt32)r.Next() % 1000000;
                var ptr = allo.alloc(rlen);
                ptrs.Add(ptr);

                string str = "alloc ptr ptr:{0}, len:{1}";
                str = string.Format(str, ptr, rlen);
                Console.WriteLine(str);
            }

            foreach (var ptr in ptrs)
            {
                string str = "free ptr ptr:{0}, len:{1}";
                str = string.Format(str, ptr, allo.getLen(ptr));
                Console.WriteLine(str);
                allo.free(ptr);
            }
            ptrs.Clear();
        }
            
    }
}

