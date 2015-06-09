using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace networkLib
{
    public class ServerBase
    {

        Socket mSocketLisener = null;

        private Dictionary<int, sessionBase> mSessions = new Dictionary<int, sessionBase>();

        public IEnumerable<sessionBase> sessions
        {
            get
            {
                foreach (var sess in mSessions.Values)
                {
                    yield return sess;
                }
            }
        }

        Queue<System.Action> mActionQ = new Queue<System.Action>();
        static int idCounter = 0;
        int newID()
        {
            idCounter++;
            return idCounter;
        }

        Thread mThreadBind = null;
        Thread mThreadListen = null;
        bool mThreadListenExit = false;

        private List<int> toDel = new List<int>();
        public void runOnce()
        {
            while (mActionQ.Count != 0)
            {
                var act = mActionQ.Dequeue();
                act();
            }

            foreach (var s in mSessions.Values)
            {
                if (s.socket == null)
                {
                    toDel.Add(s.id);
                }
                else
                {
                    s.runOnce();
                }
            }

            foreach (var id in toDel)
            {
                mSessions.Remove(id);
                if (evtLog != null)
                {
                    evtLog(string.Format("session id={0},已经无用，删除", id));
                }
            }
            toDel.Clear();
        }


        public static ServerBase create()
        {
            ServerBase ins = new ServerBase();
            return ins;
        }

        public void bind(short port)
        {
            if (mThreadBind != null)
            {
                mActionQ.Enqueue(() =>
                {
                    if (evtLog != null)
                    {
                        evtLog(string.Format("启动监听{0}失败, 正在绑定", port));
                    }
                    if (evtErr != null)
                    {
                        evtErr(-1, string.Format("启动监听{0}失败, 正在绑定", port));
                    }
                });

                return;
            }
            if (mSocketLisener != null)
            {
                mActionQ.Enqueue(() =>
                {
                    if (evtLog != null)
                    {
                        evtLog(string.Format("启动监听{0}失败, Server已经建立", port));
                    }
                    if (evtErr != null)
                    {
                        evtErr(-1, string.Format("启动监听{0}失败, Server已经建立", port));
                    }
                });
                return;
            }

            mThreadBind = new Thread(() =>
            {
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                mSocketLisener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                mSocketLisener.Bind(new IPEndPoint(ip, port));  //绑定IP地址：端口  
                mSocketLisener.Listen(10);    //设定最多10个排队连接请求  
                mActionQ.Enqueue(() =>
                {
                    if (evtLog != null)
                    {
                        evtLog(string.Format("启动监听{0}成功", mSocketLisener.LocalEndPoint.ToString()));
                    }
                });
                try
                {
                    mThreadListen = new Thread(() =>
                    {
                        while (!mThreadListenExit)
                        {
                            Socket clientSocket = mSocketLisener.Accept();
                            int id = newID();
                            sessionBase sess = new sessionBase();
                            sess.id = id;
                            sess.socket = clientSocket;

                            var recvThread = new Thread(() =>
                            {
                                sess.evtStatError += err =>
                                {
                                    if (evtLog != null)
                                    {
                                        evtLog(err);
                                    }
                                };
                                sess.asycRun();
                            });
                            mActionQ.Enqueue(() =>
                            {
                                sess.evtStatRecv += str =>
                                {
                                    if (evtRecv != null)
                                    {
                                        evtRecv(sess.id, str);
                                    }
                                };

                                sess.evtStatError += str =>
                                {
                                    if (evtErr != null)
                                    {
                                        evtErr(sess.id, str);
                                    }
                                };
                                mSessions.Add(id, sess);
                                recvThread.Start();
                                if (evtLog != null)
                                {
                                    evtLog(string.Format("有客户端接入，id:{0}", id));
                                }
                            });
                        }
                    });
                    mThreadListen.Start();
                }
                catch (Exception ex)
                {
                    mActionQ.Enqueue(() =>
                    {
                        if (evtLog != null)
                        {
                            evtLog("连接服务器失败:" + ex.Message);
                        }
                        if (evtErr != null)
                        {
                            evtErr(-1, ex.Message);
                        }
                    });
                }

                mThreadBind = null;
            });

            mThreadBind.Start();
        }

        public event System.Action<string> evtLog;
        public event System.Action<int, string> evtRecv;
        public event System.Action<int, string> evtErr;
        //协议
        public void send(int cid, string str)
        {
            sessionBase sess = null;
            mSessions.TryGetValue(cid, out sess);
            if (sess != null)
            {
                sess.send(str);
            }
        }

        public void close()
        {
            mSocketLisener.Shutdown(SocketShutdown.Both);
            mSocketLisener.Close();
            mSocketLisener = null;
            mThreadListenExit = false;

            foreach (var s in mSessions.Values)
            {
                s.close();
            }
        }

        public void closeClient(int cid)
        {
            sessionBase sess = null;
            mSessions.TryGetValue(cid, out sess);
            if (sess != null)
            {
                sess.close();
            }
        }
    }
}
