using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Touch
{
    public class entityManager
    {
        public List<entity> ents = new List<entity>();
        public List<linker> linkers = new List<linker>();


        public void save()
        {
            //num entity
            //entity line (id, name, x, y)
            //links
            //(id1, id2)
            List<string> lines = new List<string>();
            lines.Add(ents.Count.ToString());
            for(int i = 0; i<ents.Count; ++i)
            {
                string ln = "";
                ln = string.Format("{0} {1} {2} {4}", i, ents[i].getUI().text, ents[i].getUI().Margin.Left, ents[i].getUI().Margin.Top);
                lines.Add(ln);
            }
            for(int i = 0; i<linkers.Count; ++i)
            {
                var lnk = linkers[i];
                string ln = "";
                ln = string.Format("{0} {1}", ents.IndexOf(lnk.left), ents.IndexOf(lnk.right));
                lines.Add(ln);
            }
        }

        public void load()
        {

        }


        static entityManager _ins = null;
        public static entityManager ins
        {
            get
            {
                if(_ins == null)
                {
                    _ins = new entityManager();
                    _ins.getUI();
                    _ins.getUI().PreviewKeyDown += (send, e) =>
                    {
                        if (e.Key == Key.Insert)
                        {
                            _ins.save();
                        }
                    };
                }
                return _ins;
            }
        }

        public entity addEntity(string name, float x, float y)
        {
            var ent = new entity();
            getUI().Children.Add(ent.getUI());
            ent.getUI().evtOnDrag = () => updateUI();
            ent.name = name;
            ent.x = x;
            ent.y = y;
            ent.updateView();
            ents.Remove(ent);
            ents.Add(ent);
            return ent;
        }

        public void updateUI()
        {
            linkers.ForEach(lnk =>
            {
                lnk.left = lnk.left;
                lnk.right = lnk.right;
            });
        }
        
        public linker addLinker(entity left, entity right)
        {
            var lnk = new linker();
            getUI().Children.Add(lnk.getUI());
            linkers.Add(lnk);
            lnk.left = left;
            lnk.right = right;

            return lnk;
        }

        public void removeLinker(linker lnk)
        {
            linkers.Remove(lnk);
            getUI().Children.Remove(lnk.getUI());
        }
        
        public void removeEntity(entity ent)
        {
            ents.Remove(ent);
            getUI().Children.Remove(ent.getUI());
            var lnks = linkers.Where(lnk => lnk.left == ent || lnk.right == ent).ToList();
            foreach(var lnk in lnks)
            {
                removeLinker(lnk);
            }
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
        public e_createLineSt pickSt = e_createLineSt.e_init;
        public CNodeLink curLink = null;
        public CNodeLink choosedLink = null;


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
