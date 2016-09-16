using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MiniEditor
{
    [CustomComponent(path = "DEMO/RVO", name = "静态阻挡")]
    class COMRVOObstacle : MComponent
    {
        public bool mInEdit = false;
        public bool edit
        {
            get
            {
                return mInEdit;
            }
            set
            {
                mInEdit = value;
                if(mInEdit)
                {
                    EditorFuncs.evtLeftMouseUp -= EditorFuncs_evtMouseUp;
                    EditorFuncs.evtKey -= EditorFuncs_evtKey;
                    EditorFuncs.evtLeftMouseUp += EditorFuncs_evtMouseUp;
                    EditorFuncs.evtKey += EditorFuncs_evtKey;
                }
                else
                {
                    EditorFuncs.evtLeftMouseUp -= EditorFuncs_evtMouseUp;
                    EditorFuncs.evtKey -= EditorFuncs_evtKey;
                }
            }
        }


        public bool mPickable = false;
        public bool pickable
        {
            get
            {
                return mPickable;
            }
            set
            {
                mPickable = value;
                if (mPickable)
                {
                    EditorFuncs.evtLeftMouseDrag -= EditorFuncs_evtLeftMouseDrag;
                    EditorFuncs.evtLeftMouseDrag += EditorFuncs_evtLeftMouseDrag;
                }
                else
                {
                    EditorFuncs.evtLeftMouseDrag -= EditorFuncs_evtLeftMouseDrag;
                }
            }
        }

        public string points
        {
            get
            {
                List<string> strPts = new List<string>();
                foreach(var p in mObstacle)
                {
                    strPts.Add(string.Format("{0:0.00} {1:0.00}", p.x(), p.y()));
                }
                return string.Join("|", strPts);
            }
            set
            {
                try
                {
                    var strPts = value.Split('|');
                    List<RVO.Vector2> pts = new List<RVO.Vector2>();
                    foreach (var p in strPts)
                    {
                        var xy = p.Split(' ');
                        double x = Convert.ToDouble(xy[0]);
                        double y = Convert.ToDouble(xy[1]);
                        pts.Add(new RVO.Vector2((float)x, (float)y));
                    }
                    mObstacle = pts;
                    reflush();
                }
                catch(Exception ex)
                {
                    MLogger.error(ex.ToString());
                }
            }
        }

        private void EditorFuncs_evtLeftMouseDrag(double arg1, double arg2)
        {
            mObstacle = mObstacle.Select(v => new RVO.Vector2(v.x() + (float)arg1, v.y() + (float)arg2)).ToList();
            reflush();
        }

        public string color
        {
            get
            {
                return (getObstacleUI().Fill as SolidColorBrush).Color.ToString();
            }
            set
            {
                try
                {
                    SolidColorBrush brush =
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
                    getObstacleUI().Fill = brush;
                }
                catch (Exception ex)
                {

                }
            }
        }

        public List<RVO.Vector2> mObstacle = new List<RVO.Vector2>();
        public void addPoint(Double x, Double y)
        {
            mObstacle.Add(new RVO.Vector2((float)x, (float)y));
        }

        public void removePoint()
        {
            if(mObstacle.Count > 0)
                mObstacle.RemoveAt(mObstacle.Count - 1);
        }

        Polygon mPolygon = null;
        Polygon getObstacleUI()
        {
            if(mPolygon == null)
            {
                mPolygon = new Polygon();
                mPolygon.Fill = new SolidColorBrush(Colors.Blue);
                mPolygon.Stroke = new SolidColorBrush(Colors.White);
                mPolygon.StrokeThickness = 1;
            }
            return mPolygon;
        }

        void reflush()
        {
            mPolygon.SetValue(Canvas.LeftProperty, (double)0);
            mPolygon.SetValue(Canvas.TopProperty, (double)0);
            PointCollection polygonPoints = new PointCollection();
            foreach (var p in mObstacle)
            {
                var drawp = new System.Windows.Point((double)p.x(), (double)p.y());
                polygonPoints.Add(drawp);
            }
            mPolygon.Points = polygonPoints;
        }

        public void close()
        {
            edit = false;
            pickable = false;
        }

        public override void editorAwake()
        {
            var ui = getObstacleUI();
            if(ui.Parent == null)
            {
                EditorFuncs.getMapPage().addItem(ui);
            }
        }

        //remove component或disable时回调
        public override void editorSleep()
        {
            var ui = getObstacleUI();
            if (ui.Parent != null)
            {
                EditorFuncs.getMapPage().removeItem(ui);
            }
            close();
        }

        //进入editor object时回调
        public override void editorInit()
        {
            var ui = getObstacleUI();
            if (ui.Parent == null)
            {
                EditorFuncs.getMapPage().addItem(ui);
            }
        }

        private void EditorFuncs_evtKey(System.Windows.Input.Key k)
        {
            if(mInEdit)
            {
                if (k == System.Windows.Input.Key.Z)
                {
                    removePoint();
                    reflush();
                }
            }
        }

        private void EditorFuncs_evtMouseUp(double arg1, double arg2)
        {
            if(mInEdit)
            {
                mObstacle.Add(new RVO.Vector2((float)arg1, (float)arg2));
                reflush();
            }
        }

        //当前editor object时每祯循环回调
        public override void editorUpdate()
        {

        }

        //离开editor object时回调
        public override void editorExit()
        {
            var ui = getObstacleUI();
            if (ui.Parent == null)
            {
                EditorFuncs.getMapPage().addItem(ui);
            }
            close();
        }
    }
}
