using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_MiniGame
{
    class Program
    {
        static void Main(string[] args)
        {
            MDriver.init();
            //Stopwatch mStopWatch = new Stopwatch();
            //mStopWatch.Start();
            while (!MRuntime.exit)
            {
                MDriver.update();
            }
            MDriver.exit();
        }
    }
}
