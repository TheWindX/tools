using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MiniEditor
{
    [RequireCom(typeof(COMAgent))]
    [CustomComponent(path = "DEMO/RVO", name = "agent物体编辑")]
    class COMEditagent : MComponent
    {
        [Description]
        public string desc
        {
            get
            {
                return "map双击修改 target";
            }
        }

        Line mLineUI = null;
        UIMapObjectTarget mTargeUI = null;
        public Line getTargetLineUI()
        {
            if (mLineUI == null)
            {
                mLineUI = new Line() { Stroke = new SolidColorBrush(Colors.Blue), StrokeThickness = 2 };
            }
            return mLineUI;
        }
        public UIMapObjectTarget getTargetUI()
        {
            if(mTargeUI == null)
            {
                mTargeUI = new UIMapObjectTarget();
            }
            return mTargeUI;
        }

        public override void editorAwake()
        {
            
        }

        public override void editorSleep()
        {
            var mapUI = EditorFuncs.getMapPage();
            mapUI.MouseDoubleClick -= MapUI_MouseDoubleClick;
            mapUI.mCanvas.Children.Remove(getTargetUI());
            mapUI.mCanvas.Children.Remove(getTargetLineUI());
        }

        public override void editorInit()
        {
            var mapUI = EditorFuncs.getMapPage();
            mapUI.MouseDoubleClick += MapUI_MouseDoubleClick;
            mapUI.mCanvas.Children.Add(getTargetUI());
            mapUI.mCanvas.Children.Add(getTargetLineUI());
        }

        public override void editorUpdate()
        {
            var agent = getComponent<COMAgent>();
            var mapObj = getComponent<COMMapObject>();
            var targetUI = getTargetUI();
            var lineUI = getTargetLineUI();
            targetUI.x = agent.targetX;
            targetUI.y = agent.targetY;
            lineUI.X1 = targetUI.x;
            lineUI.Y1 = targetUI.y;
            lineUI.X2 = mapObj.x;
            lineUI.Y2 = mapObj.y;
        }

        public override void editorExit()
        {
            var mapUI = EditorFuncs.getMapPage();
            mapUI.MouseDoubleClick -= MapUI_MouseDoubleClick;
            mapUI.mCanvas.Children.Remove(getTargetUI());
            mapUI.mCanvas.Children.Remove(getTargetLineUI());
        }

        private void MapUI_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point p = EditorFuncs.getMousePositionInMap();
            var agent = getComponent<COMAgent>();
            agent.targetX = p.X;
            agent.targetY = p.Y;
        }
    }
}
