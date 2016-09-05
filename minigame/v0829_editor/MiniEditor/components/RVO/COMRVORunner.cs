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
        public System.Action addAgent
        {
            get
            {
                return () =>
                {
                    var obj = EditorWorld.createObject(getEditorObject(), "agent");
                    obj.addComponent<COMEditagent>();
                    EditorFuncs.getItemListPage().insertEditorItem(obj);
                };
            }
        }

        private bool mInit = false;
        public System.Action run
        {
            get
            {
                return () =>
                {
                    mInit = !mInit;
                    if (!mInit) return;

                    var scenario = getComponent<COMRVOScenario>();
                    Simulator.Instance.setTimeStep((float)scenario.timeStep);
                    Simulator.Instance.setAgentDefaults((float)scenario.neighbordist, (int)scenario.maxNeighbors,
                        (float)scenario.timeHorizon, (float)scenario.timeHorizonObst, (float)scenario.radius,
                        (float)scenario.maxSpeed,
                        new Vector2(0.0f, 0.0f));
                    var nodes = getEditorObject().children;
                    foreach (var node in nodes)
                    {
                        var agent = node.getComponent<COMAgent>();
                        var agentMapObj = node.getComponent<COMMapObject>();
                        int id = Simulator.Instance.addAgent(new Vector2((float)agentMapObj.x, (float)agentMapObj.y));
                        agent.agentID = id;
                    }
                };
            }
        }

        public override void editorInit()
        {
            
        }

        public override void editorUpdate()
        {
            if(mInit)
            {
                //update agent pos
                Console.WriteLine(Simulator.Instance.getGlobalTime());
                var nodes = getEditorObject().children;
                foreach (var node in nodes)
                {
                    var agent = node.getComponent<COMAgent>();
                    var agentMapObj = node.getComponent<COMMapObject>();
                    var pos = Simulator.Instance.getAgentPosition(agent.agentID);
                    agentMapObj.x = (double)pos.x();
                    agentMapObj.y = (double)pos.y();
                    //Console.WriteLine("{0}, {1}", agentMapObj.x, agentMapObj.y);
                }

                //update prevelocity
                foreach (var node in nodes)
                {
                    var agent = node.getComponent<COMAgent>();
                    var agentMapObj = node.getComponent<COMMapObject>();

                    var goal = new Vector2((float)agent.targetX, (float)agent.targetY);
                    var now = new Vector2((float)agentMapObj.x, (float)agentMapObj.y);
                    Vector2 goalVector = goal - now;
                    //if (RVOMath.absSq(goalVector) > 1.0f)
                    //{
                    //    goalVector = RVOMath.normalize(goalVector);
                    //}
                    Simulator.Instance.setAgentPrefVelocity(agent.agentID, goalVector);
                }
                Simulator.Instance.doStep();
                //update prevelocity
            }
        }

        public override void editorExit()
        {
            
        }


    }
}
