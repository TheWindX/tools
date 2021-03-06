﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace ns_vision.ns_utils
{
    public class TimerManager
    {
        public class UHeap<T> //where T : class
        {
            List<T> m_arr = new List<T>();
            Func<T, T, bool> m_pred;
            Action<T, int> m_move;

            void swap(int idx1, int idx2)
            {
                var e1 = m_arr[idx1];
                var e2 = m_arr[idx2];
                m_arr[idx1] = e2;
                m_arr[idx2] = e1;
                m_move(e1, idx2);
                m_move(e2, idx1);
            }

            public UHeap(Func<T, T, bool> pred, Action<T, int> move)
            {
                m_pred = pred;
                if (m_pred == null)
                    m_pred = (a, b) => { return true; };
                m_move = move;
                if (m_move == null)
                    m_move = (e, p) => { };
            }

            public void update(int p)
            {
                rise(p);
                sink(p);
            }


            public void sink(int p)
            {
                int sz = m_arr.Count;

                for (int pos = p + 1; ; )
                {
                    int p1 = pos << 1;
                    int p2 = p1 + 1;
                    if (p1 > sz) return;
                    int upPos = pos - 1;
                    int leftPos = p1 - 1;
                    T up = m_arr[upPos];
                    T left = m_arr[leftPos];
                    if (p2 > sz)
                    {
                        if (m_pred(left, up)) { swap(upPos, leftPos); }
                        pos = p1;
                        continue;
                    }
                    else
                    {
                        T right = m_arr[p1];
                        if (m_pred(left, right) && m_pred(left, up))
                        {
                            swap(upPos, leftPos);
                            pos = p1;
                        }
                        else if (m_pred(right, up))
                        {
                            swap(upPos, p1);
                            pos = p2;
                        }
                        else
                        {
                            return;
                        }
                    }
                }

            }

            public void rise(int p)
            {
                for (int pos = p + 1; ; )
                {
                    int up_pos;

                    up_pos = pos >> 1;
                    if (up_pos == 0) return;
                    var up = m_arr[up_pos - 1];
                    var low = m_arr[pos - 1];
                    if (m_pred(low, up))
                    {
                        swap(up_pos - 1, pos - 1);
                    }
                    pos = up_pos;
                }
            }

            public void push(T elem)
            {
                m_arr.Add(elem);
                int sz = m_arr.Count();
                m_move(elem, sz - 1);

                rise(sz - 1);
            }

            public bool empty()
            {
                return m_arr.Count() == 0;
            }

            public T top()
            {
                if (empty()) return default(T);
                return m_arr[0];
            }
            public void pop()
            {
                if (empty()) return;
                var e = m_arr.Last();
                m_arr[0] = e;
                m_move(e, 0);
                m_arr.RemoveAt(m_arr.Count - 1);
                //sink
                sink(0);
            }

            public void clear()
            {
                m_arr.Clear();
            }

            public int size()
            {
                return m_arr.Count;
            }
        };

        class TimeContext
        {
            public int mID = 0;
            public int mIndex = 0;
            Action<UInt32, UInt32, bool> mIntevalHandler = null;
            UInt32 mStartTime;
            UInt32 mDuration;
            UInt32 mInterval;

            public bool tillEnd(UInt32 now)
            {
                var ret = now >= mEndCount;
                if (ret)
                {
                    mValid = false;
                    mIntevalHandler(0, now - mStartTime, true);
                }
                return ret;
            }

            public bool tillInterval(UInt32 now)
            {
                bool ret = now > mIntervalCount;
                if (ret)
                {
                    uint det = now - mIntervalCount;
                    mIntervalCount = mIntervalCount + mInterval;
                    //var t = ((float)(now - mStartTime)) / mDuration;
                    mIntevalHandler(det + mInterval, now - mStartTime, false);
#if NO_ACCUMULATE
                    if (mIntervalCount < now)
                    {
                        mIntervalCount = now + 1;
                    }
#endif
                }
                return ret;
            }

            UInt32 mIntervalCount;
            UInt32 mEndCount;

            public bool mValid = true;

            internal TimeContext(int id, UInt32 now, Action<UInt32, UInt32, bool> handler, UInt32 duration, UInt32 interval)
            {
                mID = id;
                mIntevalHandler = handler;
                mStartTime = now;
                mDuration = duration;
                mInterval = interval;

                mEndCount = mStartTime + mDuration;
                mIntervalCount = mStartTime + mInterval;

                if (interval == UInt32.MaxValue)
                    mIntervalCount = UInt32.MaxValue;
                if (mDuration == UInt32.MaxValue)
                    mEndCount = UInt32.MaxValue;

                mValid = true;
            }

            UInt32 nextTime
            {
                get
                {
                    if (mIntervalCount < mEndCount)
                    {
                        return mIntervalCount;
                    }
                    else return mEndCount;
                }
            }


            public static bool comp(TimeContext a, TimeContext b)
            {
                if (!a.mValid)
                {
                    if (!b.mValid)
                    {
                        return a.mID < b.mID;
                    }
                    else return true;
                }

                if (!b.mValid)
                {
                    return false;
                }

                uint an = a.nextTime;
                uint bn = b.nextTime;
                if (an < bn)
                {
                    return true;
                }
                else if (a.nextTime == b.nextTime)
                {
                    return a.mID < b.mID;
                }
                return false;
            }

            public static void move(TimeContext a, int idx)
            {
                a.mIndex = idx;
            }
        }

        public TimerManager(Func<UInt32> t, string id)
        {
            mID = id;
        }
        public string mID = "";
        Dictionary<int, TimeContext> mID2Interpolation = new Dictionary<int, TimeContext>();

        UHeap<TimeContext> mInterpolates = new UHeap<TimeContext>(TimeContext.comp, TimeContext.move);


        int mIDCount = 0;


        //timer as it in html5
        public int setTimeout(Action<UInt32> handler, UInt32 duration)
        {
            UInt32 start = TimerManager.now();
            var elem = new TimeContext(++mIDCount, start, (i, t, b) => { handler(t); }, duration, UInt32.MaxValue);
            mInterpolates.push(elem);
            mID2Interpolation.Add(mIDCount, elem);
            //RuntimeUtil.Instance.logLine("add timer:" + mIDCount);
            return mIDCount;
        }

        public int setInterval(Action<UInt32, UInt32> intervalHandler, UInt32 interval, Action<UInt32> endHandler = null, UInt32 duration = UInt32.MaxValue)
        {
            if (intervalHandler == null) intervalHandler = (i, n) => { };
            if (endHandler == null) endHandler = (n) => { };

            UInt32 start = TimerManager.now();
            var elem = new TimeContext(++mIDCount, start, (i, t, b) => { if (b) endHandler(t); else intervalHandler(i, t); }, duration, interval);
            mInterpolates.push(elem);
            mID2Interpolation.Add(mIDCount, elem);
            //RuntimeUtil.Instance.logLine("add timer:" + mIDCount);
            return mIDCount;
        }

        public bool clearTimer(int id)
        {
            TimeContext elem = null;
            if (mID2Interpolation.TryGetValue(id, out elem))
            {
                elem.mValid = false;
                mInterpolates.update(elem.mIndex);
                return true;
            }
            return false;
        }

        public int timerCount()
        {
            int sz = mInterpolates.size();
            return sz;
        }

        public void clearAll()
        {
            mID2Interpolation.Clear();
            mInterpolates.clear();
        }

        public void tick()
        {
            UInt32 now = TimerManager.now();

            for (; ; )
            {
                var t = mInterpolates.top();
                if (t == null) break;
                if (!t.mValid)
                {
                    mID2Interpolation.Remove(t.mID);
                    mInterpolates.pop();
                }
                else
                {
                    if (t.tillEnd(now))
                    {
                        mID2Interpolation.Remove(t.mID);
                        mInterpolates.pop();//这里保证是还在top上

                    }
                    else if (t.tillInterval(now))
                    {
                        mInterpolates.update(t.mIndex);
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }

        static SortedDictionary<string, TimerManager> mID2Timer = new SortedDictionary<string, TimerManager>();
        static SortedDictionary<string, TimerManager> mPreID2Timer = new SortedDictionary<string, TimerManager>();
        static bool mIDTimerChanged = false;
        static Func<UInt32> mGlobalTimer;
        public static UInt32 now()
        {
            if (mGlobalTimer != null) return mGlobalTimer();
            return 0;
        }
        public static void init(Func<UInt32> t)
        {
            mGlobalTimer = t;
        }

        public static bool isInit()
        {
            return mGlobalTimer != null;
        }

        public static TimerManager get(string level)//DEFAULT
        {
            TimerManager mout;
            if (mID2Timer.TryGetValue(level, out mout))
            {
                return mout;
            }
            else
            {
                if (mPreID2Timer.TryGetValue(level, out mout))
                {
                    return mout;
                }

                var t = new TimerManager(mGlobalTimer, level);
                if (!mPreID2Timer.ContainsKey(level))
                    mPreID2Timer.Add(level, t);
                mIDTimerChanged = true;
                return t;
            }
        }

        public static void tick(string id)
        {
            TimerManager timer = null;
            if(mID2Timer.TryGetValue(id, out timer) )
            {
                timer.tick();
            }
            if (mIDTimerChanged)
            {
                foreach (var elem in mPreID2Timer)
                {
                    mID2Timer.Add(elem.Key, elem.Value);
                }
                mPreID2Timer.Clear();
                mIDTimerChanged = false;
            }
        }
    }
}

//first inherit (time func)
//then tick every update
//use it
