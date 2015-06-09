using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleNetworkLib;
using System.Threading;

namespace clientTest
{
    class Program
    {
        static Queue<System.Action> mAQ = new Queue<Action>();
        static void Main(string[] args)
        {
            var c = SimpleNetworkLib.ClientBase.create();
            c.evtLog += str =>
                {
                    Console.WriteLine(str);
                };

            c.connect("127.0.0.1", 10021);

            c.session.evtStatError += str =>
                {
                    Console.WriteLine(str);
                };
            c.session.evtStatRecv += str =>
                {
                    Console.WriteLine(str);
                };

            //input thread
            var inputThread = new Thread(() =>
                {
                    while(true)
                    {
                        var ln = Console.ReadLine();
                        c.session.send(ln);
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
                c.runOnce();
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
