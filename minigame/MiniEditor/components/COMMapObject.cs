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
using System.Windows.Controls;
using System.Windows.Media;

namespace MiniEditor
{
    [CustomComponent(path = "CORE", name ="地图物体")]
    public class COMMapObject : MComponent
    {
        private double mX = 20;
        public double x
        {
            get
            {
                return mX;
            }
            set
            {
                mX = value;
                getMapUIItem().x = value;
            }
        }

        private double mY = 20;
        public double y
        {
            get
            {
                return mY;
            }
            set
            {
                mY = value;
                getMapUIItem().y = value;
            }
        }

        private double mRadius = 20;
        public double radius
        {
            get
            {
                return mRadius;
            }
            set
            {
                mRadius = value;
                getMapUIItem().radius = value;
            }
        }

        public string color
        {
            get
            {
                return (getMapUIItem().background as SolidColorBrush).Color.ToString();
            }
            set
            {
                try
                {
                    SolidColorBrush brush =
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
                    getMapUIItem().background = brush;
                }
                catch(Exception ex)
                {
                    
                }
            }
        }

        UIMapObj uiObj = null;
        public UIMapObj getMapUIItem()
        {
            if(uiObj == null)
            {
                uiObj = new UIMapObj() { x=0, y=0, radius = 15};
                uiObj.editorObject = getEditorObject();
            }
            return uiObj;
        }

        public override void editorAwake()
        {
            EditorFuncs.getMapPage().addItem(getMapUIItem());
        }

        public override void editorSleep()
        {
            EditorFuncs.getMapPage().removeItem(getMapUIItem());
            //EditorFuncs.evtLeftMouseDrag -= EditorFuncs_evtLeftMouseDrag;
        }

        public override void editorInit()
        {
            getMapUIItem().isPicked = true;
            //EditorFuncs.evtLeftMouseDrag -= EditorFuncs_evtLeftMouseDrag;
            //EditorFuncs.evtLeftMouseDrag += EditorFuncs_evtLeftMouseDrag;
        }

        //private void EditorFuncs_evtLeftMouseDrag(double arg1, double arg2)
        //{
        //    x += arg1;
        //    y += arg2;
        //}

        public override void editorUpdate()
        {
            //MLogger.info("editorUpdate: {0}", GetType().Name);
        }

        public override void editorExit()
        {
            getMapUIItem().isPicked = false;
            //EditorFuncs.evtLeftMouseDrag -= EditorFuncs_evtLeftMouseDrag;
        }
    }
}
