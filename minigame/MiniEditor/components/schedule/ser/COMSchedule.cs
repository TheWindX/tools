using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    class COMSchedule : MComponent
    {
        //实现树正确结构的检查
        public virtual bool check()
        {
            //TODO
            return true;
        }

        //初始化，参数apply
        public virtual void scheduleInit()
        {
            mState = ESTATE.e_inited;
        }

        
        //未完成update返回false， 完成返回true
        public virtual bool scheduleUpdate()
        {
            return true;
        }

        
        //返回执行是否成功, 此调用完成后必须保证reset 状态
        public virtual bool scheduleExit()
        {
            mState = ESTATE.e_uninit;
            return true;
        }

        //无条件中止， 此调用完成后必须保证reset 状态
        public virtual void scheduleInterrupt()
        {
            if (mState == ESTATE.e_uninit) return;
            mState = ESTATE.e_uninit;
            //foreach(COMSchedule beh in scheduleGetChildren() )
            //{
            //    try
            //    {
            //        if(beh.getState() == ESTATE.e_inited)
            //        {
            //            beh.scheduleInterrupt();
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        MLogger.error(ex.ToString());
            //    }
            //}
        }

        //子任务
        public IEnumerable<COMSchedule> scheduleGetChildren()
        {
            var behs = getEditorObject().children.Select(obj => obj.getComponent<COMSchedule>());
            foreach (COMSchedule beh in behs)
            {
                if (beh != null)
                {
                    yield return beh;
                }
            }
        }

        //父任务
        public COMSchedule scheduleGetParent()
        {
            var pobj = getEditorObject().parent;
            if (pobj == null) return null;
            return pobj.getComponent<COMSchedule>();
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

        //////overrides
        //add component或enable时回调
        public override void editorAwake()
        {
            var attr = GetType().GetCustomAttribute<CustomComponentAttribute>();
            getComponent<COMEditorObject>().name = attr.name;
        }

    }
}
