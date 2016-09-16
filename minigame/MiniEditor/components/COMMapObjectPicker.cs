using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MiniEditor
{
    [CustomComponent(path = "CORE", name = "地图物体捡选")]
    [RequireCom(typeof(COMMapObject))]
    public class COMMapObjectPicker : MComponent
    {
        private void COMMapObjectPicker_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditorFuncs.getItemListPage().pickEditObject(getEditorObject());
        }

        public override void editorAwake()
        {
            var mapObj = getComponent<COMMapObject>();
            mapObj.getMapUIItem().MouseDown -= COMMapObjectPicker_MouseDown;
            mapObj.getMapUIItem().MouseDown += COMMapObjectPicker_MouseDown;
        }

        public override void editorSleep()
        {
            var mapObj = getComponent<COMMapObject>();
            mapObj.getMapUIItem().MouseDown -= COMMapObjectPicker_MouseDown;
        }

        public override void editorInit()
        {
            var mapObj = getComponent<COMMapObject>();
            mapObj.getMapUIItem().isPicked = true;
            EditorFuncs.evtLeftMouseDrag -= EditorFuncs_evtLeftMouseDrag;
            EditorFuncs.evtLeftMouseDrag += EditorFuncs_evtLeftMouseDrag;
        }

        public override void editorExit()
        {
            var mapObj = getComponent<COMMapObject>();
            mapObj.getMapUIItem().isPicked = false;
            EditorFuncs.evtLeftMouseDrag -= EditorFuncs_evtLeftMouseDrag;
        }

        private void EditorFuncs_evtLeftMouseDrag(double arg1, double arg2)
        {
            var mapObj = getComponent<COMMapObject>();
            mapObj.x += arg1;
            mapObj.y += arg2;
        }
    }
}
