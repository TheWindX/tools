﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MiniEditor
{
    [CustomComponent(path = "global", name ="地图物体")]
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
                EditorFuncs.instance().getMapPage().addItem(uiObj);
            }
            return uiObj;
        }

        public override void editorInit()
        {
            getMapUIItem().isPicked = true;
        }

        public override void editorUpdate()
        {
            //MLogger.info("editorUpdate: {0}", GetType().Name);
        }

        public override void editorExit()
        {
            getMapUIItem().isPicked = false;
        }
    }
}
