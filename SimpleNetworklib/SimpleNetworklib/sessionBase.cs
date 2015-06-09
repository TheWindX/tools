using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace networkLib
{
    public class sessionBase
    {
        public int id = 0;

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
            e_len,
            e_data,
        }

        protoState mStat = protoState.e_uninit;
        public event System.Action<string> evtStatError;
        public event System.Action<string> evtStatRecv;
        int mLen = 0;
        Queue<byte> datas = new Queue<byte>();

        void recvState()
        {
            if (mSocket == null) return;
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
                        return;
                    }
                    mStat = protoState.e_magic1;
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
                        return;
                    }
                    mStat = protoState.e_len;
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

                    mLen = BitConverter.ToInt32(bsLen, 0);
                    mStat = protoState.e_data;
                }
            }
            else if (mStat == protoState.e_data)
            {
                if (datas.Count >= mLen)
                {
                    byte[] bs = new byte[mLen];
                    for (int i = 0; i < mLen; ++i)
                    {
                        bs[i] = datas.Dequeue();
                    }

                    string str = netUtils.GetString(bs);
                    if (evtStatRecv != null)
                    {
                        evtStatRecv(str);
                    }
                    mStat = protoState.e_magic0;
                }
            }
        }

        byte[] buff = new byte[8096];

        Queue<System.Action> mActionQ = new Queue<System.Action>();

        public void runOnce()
        {
            recvState();
            while (mActionQ.Count != 0)
            {
                var act = mActionQ.Dequeue();
                act();
            }
        }

        public void asycRun()
        {
            bool exit = false;
            while (!exit)
            {
                try
                {
                    int receiveNumber = mSocket.Receive(buff);
                    mActionQ.Enqueue(() =>
                    {
                        for (int i = 0; i < receiveNumber; ++i)
                        {
                            datas.Enqueue(buff[i]);
                        }
                    });
                }
                catch (Exception ex)
                {
                    exit = true;
                    mActionQ.Enqueue(() =>
                    {
                        if (evtStatError != null)
                        {
                            evtStatError(string.Format("接受数据失败:{0}", ex.Message));
                        }
                        close();
                    });
                }
            }
        }

        public void send(string str)
        {
            if (mSocket != null)
            {
                List<byte> sbs = new List<byte>();
                sbs.Add(127);
                sbs.Add(128);
                var bs = netUtils.GetBytes(str);
                var bsLen = BitConverter.GetBytes(bs.Length);
                sbs.AddRange(bsLen);
                sbs.AddRange(bs);
                mSocket.Send(sbs.ToArray());
            }
        }

        public void close()
        {
            if (mSocket != null)
            {
                mSocket.Shutdown(SocketShutdown.Both);
                mSocket.Close();
            }
            mSocket = null;

            datas.Clear();
            mStat = protoState.e_uninit;
        }
    }
}
