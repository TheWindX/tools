using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using networkLib;
using System.Threading;

namespace ServerTest
{
    class Program
    {
        static Queue<System.Action> mAQ = new Queue<Action>();
        static void Main(string[] args)
        {
            var s = networkLib.ServerBase.create();
            s.bind(10021);
            s.evtLog += str =>
                {
                    Console.WriteLine(str);
                };

            s.evtErr += (id, str) =>
                {
                    Console.WriteLine(string.Format("{0} error:{1}", id, str));
                };

            s.evtRecv += (id, str) =>
                {
                    Console.WriteLine(string.Format("recv from {0} message:{1}", id, str));
                };

            //input thread
            var inputThread = new Thread(() =>
                {
                    while (true)
                    {
                        var ln = Console.ReadLine();
                        s.sessions.ToList().ForEach(sess =>
                        {
                            sess.send(ln);
                        });
                    }
                });
            inputThread.Start();
            while(true)
            {
                while (mAQ.Count != 0)
                {
                    var act = mAQ.Dequeue();
                    act();
                }
                s.runOnce();
                System.Threading.Thread.Sleep(1);
            }


        }

        static void s_evtLog(string obj)
        {
            Console.WriteLine(obj);
        }

        static void s_evtAccept(int obj)
        {
            Console.WriteLine(string.Format("{0} is accept", obj));
        }
    }
}
