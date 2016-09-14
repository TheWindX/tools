using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
    未完成update返回false， 完成返回true,
    完成后，exit返回是否成功
    */
    class COMBeh : MComponent
    {

        public virtual void behInit()
        {

        }

        public virtual bool behUpdate()
        {
            return false;
        }

        public virtual bool behExit()
        {
            return true;
        }

        public virtual void behInterrupt()
        {
        }

        public virtual void behOnResult(COMBeh child, bool res)
        {

        }

        public virtual IEnumerable<COMBeh> behChildren()
        {
            return new List<COMBeh>();
        }

    }
}
