using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SimpleNetworkLib
{
    public class sessionBase
    {
        public int id = 0;
        private static Int64 serialNumCounter = 0;
        private static Int64 genSerialNum()
        {
            serialNumCounter++;
            return serialNumCounter;
        }

        private Socket mSocket = null;

        public Socket socket
        {
            get
            {
                return mSocket;
            }
            set
            {
                close();
                mStat = protoState.e_magic0;
                mSocket = value;
            }
        }

        enum protoState
        {
            e_uninit,
            e_magic0,
            e_magic1,
            e_seriNO,
            e_len,
            e_data,
        }

        protoState mStat = protoState.e_uninit;
        public event System.Action<string> evtStatError;
        public event System.Action<string> evtLog;
        public event System.Action<Int64, string> evtStatRecv;
        int mRecvLen = 0;
        Int64 mRecvSN = 0;
        Queue<byte> datas = new Queue<byte>();

        bool recvState()
        {
            if (mSocket == null) return false;
            if (mStat == protoState.e_magic0)
            {
                if (datas.Count >= 1)
                {
                    var m0 = datas.Dequeue();
                    if (m0 != 127)
                    {
                        if (evtStatError != null)
                        {
                            evtStatError(string.Format("接受数据失败:m0 == {0}", m0));
                        }
                        close();
                        return false;
                    }
                    mStat = protoState.e_magic1;
                    return true;
                }
            }
            else if (mStat == protoState.e_magic1)
            {
                if (datas.Count >= 1)
                {
                    var m1 = datas.Dequeue();
                    if (m1 != 128)
                    {
                        if (evtStatError != null)
                        {
                            evtStatError(string.Format("接受数据失败:m1 == {0}", m1));
                        }
                        close();
                        return false;
                    }
                    mStat = protoState.e_seriNO;
                    return true;
                }
            }
            else if (mStat == protoState.e_seriNO)
            {
                if (datas.Count >= 8)
                {
                    var bsSN = new byte[8];
                    bsSN[0] = datas.Dequeue();
                    bsSN[1] = datas.Dequeue();
                    bsSN[2] = datas.Dequeue();
                    bsSN[3] = datas.Dequeue();
                    bsSN[4] = datas.Dequeue();
                    bsSN[5] = datas.Dequeue();
                    bsSN[6] = datas.Dequeue();
                    bsSN[7] = datas.Dequeue();

                    mRecvSN = BitConverter.ToInt64(bsSN, 0);
                    mStat = protoState.e_len;
                    return true;
                }
            }
            else if (mStat == protoState.e_len)
            {
                if (datas.Count >= 4)
                {
                    var bsLen = new byte[4];
                    bsLen[0] = datas.Dequeue();
                    bsLen[1] = datas.Dequeue();
                    bsLen[2] = datas.Dequeue();
                    bsLen[3] = datas.Dequeue();

                    mRecvLen = BitConverter.ToInt32(bsLen, 0);
                    mStat = protoState.e_data;
                    return true;
                }
            }
            else if (mStat == protoState.e_data)
            {
                if (datas.Count >= mRecvLen)
                {
                    byte[] bs = new byte[mRecvLen];
                    for (int i = 0; i < mRecvLen; ++i)
                    {
                        bs[i] = datas.Dequeue();
                    }

                    string str = netUtils.GetString(bs);
                    //try//TODO...
                    {
                        if (evtStatRecv != null)
                        {
                            evtStatRecv(mRecvSN, str);
                        }
                    }                    //catch(Exception ex)
                    //{
                    //    if(evtStatError != null)
                    //    {
                    //        evtStatError(ex.Message);
                    //    }
                    //}

                    
                    mStat = protoState.e_magic0;
                    return true;
                }
            }
            return false;
        }

        byte[] buff = new byte[8096];

        Queue<System.Action> mActionQ = new Queue<System.Action>();

        public void runOnce()
        {
            while (recvState()) { }
            while (mActionQ.Count != 0)
            {
                var act = mActionQ.Dequeue();
                try
                {
                    act();
                }
                catch(Exception ex)
                {
                    if(evtStatError != null)
                    {
                        evtStatError("error in sessionBase.runOnce:" + ex.Message);
                    }
                }
            }
        }

        bool mAvailable = false;

        //是否可用
        public bool available()
        {
            return mAvailable;
        }

        public void asycRun()
        {
            mAvailable = true;
            while (mAvailable)
            {
                try
                {
                    int receiveNumber = mSocket.Receive(buff);

                    var buffClone = buff.Clone() as byte[];
                    if (receiveNumber != 0)
                    {
                        mActionQ.Enqueue(() =>
                        {
                            for (int i = 0; i < receiveNumber; ++i)
                            {
                                datas.Enqueue(buffClone[i]);
                            }
                        });
                    }
                    else
                    {   
                        mAvailable = false;
                    }
                }
                catch (Exception ex)
                {
                    mAvailable = false;
                    mActionQ.Enqueue(() =>
                    {
                        if (evtStatError != null)
                        {
                            evtStatError(string.Format("接受数据失败:{0}", ex.Message));
                        }
                    });
                    close();
                }
            }
            close();
            mActionQ.Enqueue(() =>
                    {
                        if (evtLog != null) evtLog(string.Format("session 线程结束退出"));
                    });
        }

        //seriNum
        public Int64 send(string str)
        {
            var sn = genSerialNum();
            doSend(str, sn);
            return sn;
        }

        private void doSend(string str, Int64 sn)
        {
            if (available())
            {
                List<byte> sbs = new List<byte>();
                sbs.Add(127);
                sbs.Add(128);
                sbs.AddRange(BitConverter.GetBytes(sn));
                var bs = netUtils.GetBytes(str);
                var bsLen = BitConverter.GetBytes(bs.Length);
                sbs.AddRange(bsLen);
                sbs.AddRange(bs);
                mSocket.Send(sbs.ToArray());
            }
            else
            {
                //redo
                mActionQ.Enqueue(() =>
                {
                    doSend(str, sn);
                });
            }
        }

        public void close()
        {
            if (mSocket != null)
            {
                try
                {
                    mSocket.Shutdown(SocketShutdown.Both);
                    mSocket.Close();
                }
                catch(Exception ex)
                {
                    if(evtStatError != null)
                    {
                        evtStatError(string.Format("session {0} shutdown exception: {1}", id, ex.Message));
                    }
                }
                
            }
            mSocket = null;

            datas.Clear();
            mStat = protoState.e_uninit;
        }
    }
}
