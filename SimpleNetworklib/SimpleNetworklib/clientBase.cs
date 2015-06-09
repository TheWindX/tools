using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace networkLib
{
    public class ClientBase
    {
        public static ClientBase create()
        {
            var ret = new ClientBase();
            return ret;
        }

        Queue<System.Action> mActionQ = new Queue<Action>();
        public void runOnce()
        {
            while (mActionQ.Count != 0)
            {
                var act = mActionQ.Dequeue();
                act();
            }

            mSession.runOnce();
        }

        public event System.Action<string> evtLog;
        public event System.Action<string, short> evtConnectSucc;
        public event System.Action<string, short> evtConnectException;

        sessionBase mSession = new sessionBase();
        public sessionBase session
        {
            get
            {
                return mSession;
            }
        }

        Thread mConnectThread = null;
        public void connect(string addr, short port)
        {
            if (mConnectThread != null)
            {
                if (evtLog != null)
                {
                    evtLog(string.Format("正在进行连接,请稍后再连"));
                }
                return;
            }
            mConnectThread = new Thread(() =>
                {
                    IPAddress ip = IPAddress.Parse(addr);
                    mSession.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        mSession.socket.Connect(new IPEndPoint(ip, port)); //配置服务器IP与端口  
                        mActionQ.Enqueue(() =>
                            {
                                if (evtLog != null)
                                {
                                    evtLog(string.Format("连接服务器成功 {0}:{1}", addr, port));
                                }   
                                if(evtConnectSucc != null)
                                {
                                    evtConnectSucc(addr, port);
                                }
                            });
                    }
                    catch
                    {
                        if (evtLog != null)
                        {
                            evtLog(string.Format("连接服务器失败 {0}:{1}", addr, port));
                        }
                        if (evtConnectException != null)
                        {
                            evtConnectException(addr, port);
                        }
                        mSession.close();
                    }

                    mSession.asycRun();
                    mConnectThread = null;
                });
            mConnectThread.Start();
        }
    }
}

