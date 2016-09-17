/*
 * author: xiaofeng.li
 * mail: 453588006@qq.com
 * desc: 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [CustomComponent(path = "DEMO/RVO", removable =false, name = "RVO全局")]
    class COMRVOScenario : MComponent
    {
        public double timeStep
        {
            get;
            set;
        }

        [Description]
        public string description
        {
            get
            {
                return "agent defaults";
            }
        }

        public double neighbordist
        {
            get;
            set;
        }

        public double maxNeighbors
        {
            get;
            set;
        }

        public double timeHorizon
        {
            get;
            set;
        }

        public double timeHorizonObst
        {
            get;
            set;
        }

        public double radius
        {
            get;
            set;
        }

        public double maxSpeed
        {
            get;
            set;
        }

        public double velocityX
        {
            get;
            set;
        }

        public double velocityY
        {
            get;
            set;
        }

        public System.Action printme
        {
            get
            {
                return () =>
                {
                    MLogger.info("action from COMRVOScenario");
                };
            }
        }
    }
}
