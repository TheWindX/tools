using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    public class MRandom
    {
        private static Random mRandGen = null;
        private static void init()
        {
            if(mRandGen == null)
            {
                var seed = MRuntime.getTime()+10086;
                mRandGen = new Random(seed);
            }
        }

        public static int randInt()
        {
            init();
            return mRandGen.Next();
        }

        public static int randRange(int min, int max)
        {
            init();
            int ri = mRandGen.Next();
            var rDet = ri % (max - min + 1);
            return min + rDet;
        }

        public static bool randBool()
        {
            init();
            return mRandGen.Next()%2 == 0;
        }
    }
}
