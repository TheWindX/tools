using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Touch
{
    public class entityManager
    {
        public List<entity> ents = new List<entity>();
        public List<linker> linkers = new List<linker>();

        static entityManager _ins = null;
        public static entityManager ins
        {
            get
            {
                if(_ins == null)
                {
                    _ins = new entityManager();
                    _ins.getUI();
                }
                return _ins;
            }
        }

        public entity addEntity(string name, float x, float y)
        {
            var ent = new entity();
            getUI().Children.Add(ent.getUI());
            ent.name = name;
            ent.x = x;
            ent.y = y;
            ent.updateView();
            return ent;
        }


        public linker addLinker(entity left, entity right, linker lnk)
        {
            if(lnk == null)
                lnk = new linker();
            lnk.left = left;
            lnk.right = right;
            return lnk;
        }

        Grid mUI = null;
        public Grid getUI()
        {
            if(mUI == null)
            {
                mUI = (App.Current.MainWindow as MainWindow).m_panel;
                mUI.PreviewMouseLeftButtonDown += MUI_PreviewMouseLeftButtonDown;
                mUI.PreviewMouseMove += m_panel_PreviewMouseMove;
            }
            return mUI;
        }

        private void MUI_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if(Keyboard.IsKeyDown(Key.LeftCtrl) )
                {
                    var pos = Mouse.GetPosition(mUI);
                    addEntity("entity1", (float)pos.X, (float)pos.Y);
                }
            }
        }


        //state 
        public enum e_createLineSt
        {
            e_init,
            e_pickOnePoint,
            e_pickSecPoint,
        }
        public static e_createLineSt pickSt = e_createLineSt.e_init;
        public static CNodeLink curLink = null;


        private void m_panel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (pickSt == e_createLineSt.e_pickOnePoint)
            {
                curLink.end = e.GetPosition(this.getUI());
                this.getUI().UpdateLayout();
            }
        }

    }
}
