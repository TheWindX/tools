using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MiniEditor
{
    [RequireCom(typeof(ComAgent))]
    [CustomComponent(editable = false, name = "agent物体编辑")]
    class ComEditagent : MComponent
    {
        [Description]
        public string desc
        {
            get
            {
                return "双击创建 target";
            }
        }

        public UIMapObjectTarget mTargeUI = null;
        public UIMapObjectTarget getTargetUI()
        {
            if(mTargeUI == null)
            {
                mTargeUI = new UIMapObjectTarget();
            }
            return mTargeUI;
        }

        public override void editorInit()
        {
            var mapUI = EditorFuncs.instance().getMapPage();
            mapUI.MouseDoubleClick += MapUI_MouseDoubleClick;
            mapUI.mCanvas.Children.Add(getTargetUI());
        }

        public override void editorUpdate()
        {
            var agent = getComponent<ComAgent>();
            var targetUI = getTargetUI();
            targetUI.x = agent.targetX;
            targetUI.y = agent.targetY;
        }

        public override void editorExit()
        {
            var mapUI = EditorFuncs.instance().getMapPage();
            mapUI.MouseDoubleClick -= MapUI_MouseDoubleClick;
            mapUI.mCanvas.Children.Remove(getTargetUI());
        }

        private void MapUI_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point p = EditorFuncs.instance().getMousePositionInMap();
            var agent = getComponent<ComAgent>();
            agent.targetX = p.X;
            agent.targetY = p.Y;
        }

        
    }


}
