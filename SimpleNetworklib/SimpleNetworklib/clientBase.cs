﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace SimpleNetworkLib
{
    public class ClientBase
    {
        public static ClientBase create()
        {
            var ret = new ClientBase();
            return ret;
        }


        Queue<System.Action> mActionQ = new Queue<System.Action>();
        public void runOnce()
        {
            while (mActionQ.Count != 0)
            {
                var act = mActionQ.Dequeue();
                try
                {
                    act();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);//这里已经无法handle
                }
            }

            mSession.runOnce();
        }

        public event System.Action<string> evtLog;
        public event System.Action<string, short> evtConnectSucc;
        public event System.Action<string> evErr;

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
                                if (evtConnectSucc != null)
                                {
                                    evtConnectSucc(addr, port);
                                }
                            });
                    }
                    catch
                    {
                        mActionQ.Enqueue(() =>
                        {
                            if (evtLog != null)
                            {
                                evtLog(string.Format("连接服务器失败 {0}:{1}", addr, port));
                            }
                            if (evErr != null)
                            {
                                evErr(string.Format("连接服务器失败 {0}:{1}", addr, port));
                            }
                        });

                        mSession.close();
                    }
                    mConnectThread = null;
                    mSession.asycRun();
                    mActionQ.Enqueue(() =>
                    {
                        if (evtLog != null)
                        {
                            evtLog(string.Format("connect 线程退出"));
                        }
                    });
                });
            mConnectThread.Start();
        }
    }
}

