using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
     *  触发同一个物体上的COMBeh
     */
    [CustomComponent(path = "BEHAVIOR", name = "【运行】")]
    class COMBehRunner : MComponent
    {
        COMBeh mBeh = null;
        public bool update()
        {
            return mBeh.behUpdate();
        }

        //add component或enable时回调
        public override void editorAwake()
        {
            
        }

        //remove component或disable时回调
        public override void editorSleep()
        {
            run = false;
        }

        //进入editor object时回调
        public override void editorInit()
        {
            //MLogger.info("editorInit: {0}", GetType().Name);
        }
        
        //离开editor object时回调
        public override void editorExit()
        {
            run = false;
        }

        public override void editorUpdate()
        {
            if(run)
            {   
                if(update())
                {
                    run = false;
                }
            }
        }

        private bool mRun = false;
        public bool run
        {
            get
            {
                return mRun;
            }
            set
            {
                mBeh = getComponent<COMBeh>();
                if (mRun == value) return;
                mRun = value;
                if(mRun)
                {   
                    if(mBeh != null)
                    {
                        mBeh.behInit();
                    }
                }
                else
                {
                    if (mBeh != null)
                    {
                        if(mBeh.getState() == COMBeh.ESTATE.e_inited)
                        {
                            mBeh.behInterrupt();
                        }
                    }
                }
            }
        }//run

    }
}
