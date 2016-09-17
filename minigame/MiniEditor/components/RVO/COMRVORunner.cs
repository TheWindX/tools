/*
 * author: xiaofeng.li
 * mail: 453588006@qq.com
 * desc: 
 * */
using RVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [RequireCom(typeof(COMRVOScenario))]
    [CustomComponent(path = "DEMO/RVO", editable = true, name = "RVO 测试")]
    class COMRVORunner : MComponent
    {
        public System.Action addManager
        {
            get
            {
                return () =>
                {
                    var obj = EditorWorld.createObject(getEditorObject(), "obstacleM");
                    obj.addComponent<COMObstacleManager>();
                    EditorFuncs.getItemListPage().insertEditorObject(obj);

                    var obj1 = EditorWorld.createObject(getEditorObject(), "agentM");
                    obj1.addComponent<COMAgentManager>();
                    EditorFuncs.getItemListPage().insertEditorObject(obj1);
                };
            }
        }

        private bool mTurnOn = false;
        private Simulator mRunner = null;
        public bool play
        {
            get
            {
                return mTurnOn;
            }
            set
            {
                mRunner = new Simulator();
                mTurnOn = value;
                if (!mTurnOn) return;

                var scenario = getComponent<COMRVOScenario>();
                mRunner.setTimeStep((float)scenario.timeStep);
                mRunner.setAgentDefaults((float)scenario.neighbordist, (int)scenario.maxNeighbors,
                    (float)scenario.timeHorizon, (float)scenario.timeHorizonObst, (float)scenario.radius,
                    (float)scenario.maxSpeed,
                    new Vector2(0.0f, 0.0f));

                var agents = getChildrenCom<COMAgent>(getEditorObject() );
                var obstacles = getChildrenCom<COMRVOObstacle>(getEditorObject());

                foreach (var agent in agents)
                {
                    var agentMapObj = agent.getComponent<COMMapObject>();
                    int id = mRunner.addAgent(new Vector2((float)agentMapObj.x, (float)agentMapObj.y));
                    agent.agentID = id;
                }

                foreach(var obs in obstacles)
                {
                    mRunner.addObstacle(obs.mObstacle);
                }

                mRunner.processObstacles();
            }
        }

        public static IEnumerable<T> getChildrenCom<T>(EditorObject obj) where T:MComponent
        {
            foreach(var o in obj.children)
            {
                bool found = false;
                foreach(var c in o.components)
                {
                    if(c is T)
                    {
                        yield return c as T;
                        found = true;
                        break;
                    }
                }
                if(!found)
                {
                    foreach(var a in getChildrenCom<T>(o))
                    {
                        yield return a;
                    }
                }
            }
        }

        public override void editorInit()
        {
            
        }

        public override void editorUpdate()
        {
            if(mTurnOn)
            {
                //update agent pos
                Console.WriteLine(mRunner.getGlobalTime());

                var agents = getChildrenCom<COMAgent>(getEditorObject());
                foreach (var agent in agents)
                {
                    var agentMapObj = agent.getComponent<COMMapObject>();
                    var pos = mRunner.getAgentPosition(agent.agentID);
                    agentMapObj.x = (double)pos.x();
                    agentMapObj.y = (double)pos.y();
                    //Console.WriteLine("{0}, {1}", agentMapObj.x, agentMapObj.y);
                }

                //update prevelocity
                foreach (var agent in agents)
                {
                    var agentMapObj = agent.getComponent<COMMapObject>();

                    var goal = new Vector2((float)agent.targetX, (float)agent.targetY);
                    var now = new Vector2((float)agentMapObj.x, (float)agentMapObj.y);
                    Vector2 goalVector = goal - now;
                    //if (RVOMath.absSq(goalVector) > 1.0f)
                    //{
                    //    goalVector = RVOMath.normalize(goalVector);
                    //}
                    mRunner.setAgentPrefVelocity(agent.agentID, goalVector);
                }
                mRunner.doStep();
                //update prevelocity
            }
        }

        public override void editorExit()
        {
            
        }


    }
}
