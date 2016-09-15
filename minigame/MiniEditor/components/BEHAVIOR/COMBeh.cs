using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    class COMBeh : MComponent
    {
        //实现树正确结构的检查
        public virtual bool check()
        {
            //TODO
            throw new NotImplementedException();
        }

        //初始化，参数apply
        public virtual void behInit()
        {
            mState = ESTATE.e_inited;
        }

        
        //未完成update返回false， 完成返回true
        public virtual bool behUpdate()
        {
            return true;
        }

        
        //返回执行是否成功, 此调用完成后必须保证reset 状态
        public virtual bool behExit()
        {
            mState = ESTATE.e_uninit;
            return true;
        }

        //无条件中止， 此调用完成后必须保证reset 状态
        public virtual void behInterrupt()
        {
            if (mState == ESTATE.e_uninit) return;
            mState = ESTATE.e_uninit;
            //foreach(COMBeh beh in behGetChildren() )
            //{
            //    try
            //    {
            //        if(beh.getState() == ESTATE.e_inited)
            //        {
            //            beh.behInterrupt();
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        MLogger.error(ex.ToString());
            //    }
            //}
        }

        //子任务
        public IEnumerable<COMBeh> behGetChildren()
        {
            var behs = getEditorObject().children.Select(obj => obj.getComponent<COMBeh>());
            foreach (COMBeh beh in behs)
            {
                if (beh != null)
                {
                    yield return beh;
                }
            }
        }

        //父任务
        public COMBeh behGetParent()
        {
            var pobj = getEditorObject().parent;
            if (pobj == null) return null;
            return pobj.getComponent<COMBeh>();
        }

        public enum ESTATE
        {
            e_uninit,//未初始化阶段
            e_inited,//正在updating
            //e_exited,//完成退出
        }

        protected ESTATE mState = ESTATE.e_uninit;

        //获得状态
        public ESTATE getState()
        {
            return mState;
        }

    }
}
