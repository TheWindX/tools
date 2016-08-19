using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Touch
{
    public class DataBackup<Data> where Data : class, ICloneable, new()
    {
        List<Data> mDataBackup = new List<Data>() { new Data() };
        const int MAX_LEN = 5;
        int dataIdx = 0;

        public Data data
        {
            get
            {
                return mDataBackup[dataIdx];
            }
        }

        private void trim()
        {
            mDataBackup.RemoveRange(dataIdx + 1, mDataBackup.Count - dataIdx - 1);
        }

        public void backup()
        {
            trim();
            mDataBackup.Add(data.Clone() as Data);
            if (dataIdx > MAX_LEN)
            {
                mDataBackup.RemoveAt(0);
            }
            dataIdx = mDataBackup.Count - 1;
        }

        public void undo()
        {
            dataIdx--;
            if (dataIdx < 0)
                dataIdx = 0;
        }

        public void redo()
        {
            dataIdx++;
            if (dataIdx >= mDataBackup.Count)
                dataIdx = mDataBackup.Count - 1;
        }

    }

    public class entityManager
    {
        public class data : ICloneable
        {
            public List<string> mData = new List<string>();

            public object Clone()
            {
                var r = new data();
                foreach(var str in mData)
                {
                    r.mData.Add(str);
                }
                return r;
            }
        }

        public DataBackup<data> mDatas = new DataBackup<data>();

        public List<entity> ents = new List<entity>();
        public List<linker> linkers = new List<linker>();

        public List<string> serial()
        {
            List<string> lines = new List<string>();
            lines.Add(ents.Count.ToString());
            for (int i = 0; i < ents.Count; ++i)
            {
                string ln = "";
                var ent = ents[i];
                var ui = ent.getUI();
                var txt = ui.text;
                ln = string.Format("{0} {1} {2} {3}", i, ent.getUI().text, ent.getUI().Margin.Left, ent.getUI().Margin.Top);
                lines.Add(ln);
            }
            for (int i = 0; i < linkers.Count; ++i)
            {
                var lnk = linkers[i];
                string ln = "";
                ln = string.Format("{0} {1}", ents.IndexOf(lnk.left), ents.IndexOf(lnk.right));
                lines.Add(ln);
            }
            return lines;
        }

        public void unserial(IEnumerable<string> lns)
        {
            foreach(var ent in ents)
            {
                getUI().Children.Remove(ent.getUI());
            }

            foreach (var lnk in linkers)
            {
                getUI().Children.Remove(lnk.getUI());
            }
            ents.Clear();
            linkers.Clear();

            
            int st = 0;
            int entNums = 0;
            int lnNum = 0;
            List<entity> entAdd = new List<entity>();
            foreach (var ln in lns)
            {
                if (st == 0)
                {
                    entNums = int.Parse(ln);
                    st = 1;
                    continue;
                }
                else if (st == 1)
                {
                    var items = ln.Split(' ');
                    var name = items[1];
                    var x = float.Parse(items[2]);
                    var y = float.Parse(items[3]);
                    entAdd.Add(addEntity(name, x, y));
                    lnNum++;
                    if (lnNum == entNums)
                    {
                        getUI().UpdateLayout();
                        st = 2;
                    }
                    continue;
                }
                else if (st == 2)
                {
                    var items = ln.Split(' ');
                    var item1 = int.Parse(items[0]);
                    var item2 = int.Parse(items[1]);
                    addLinker(entAdd[item1], entAdd[item2]);
                }
            }
        }

        public void save()
        {
            if(File.Exists("data.txt") )
            {
                File.Copy("data.txt", "data"+ DateTime.Now.ToString("HHmmss")+".txt");
            }
            //num entity
            //entity line (id, name, x, y)
            //links
            //(id1, id2)
            File.WriteAllLines("data.txt", serial().ToArray());
        }

        public void load()
        {
            ents.Clear();
            linkers.Clear();
            getUI().Children.Clear();
            var lns = File.ReadAllLines("data.txt");
            unserial(lns);
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
                    App.Current.MainWindow.PreviewKeyDown += (send, e) =>
                    {
                        if (e.Key == Key.Insert)
                        {
                            _ins.save();
                        }
                        else if(e.Key == Key.Home)
                        {
                            _ins.load();
                        }
                    };
                }
                return _ins;
            }
        }

        object mSelect = null;
        object selected
        {
            get
            {
                return selected;
            }
            set
            {
                if(mSelect != null)
                {
                    if (mSelect is entity)
                    {
                        (mSelect as entity).unfocus();
                    }
                    else
                    {
                        (mSelect as linker).unfocus();
                    }
                }
                
                mSelect = value;
                if (mSelect != null)
                {
                    if (mSelect is entity)
                    {
                        (mSelect as entity).focus();
                    }
                    else
                    {
                        (mSelect as linker).focus();
                    }
                }
            }

        }

        


        public entity addEntity(string name, float x, float y)
        {
            var ent = entity.create();
            getUI().Children.Add(ent.getUI());
            ent.getUI().evtOnDrag = () => updateUI();
            ent.getUI().evtSelect = () =>
            {
                selected = ent;
            };
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
            var lnk = linker.create();
            lnk.getUI().evtSelect = () =>
            {
                selected = lnk;
            };
            getUI().Children.Add(lnk.getUI());
            linkers.Add(lnk);
            lnk.left = left;
            lnk.right = right;
            updateUI();
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
                mUI.PreviewMouseLeftButtonDown -= MUI_PreviewMouseLeftButtonDown;
                mUI.PreviewMouseLeftButtonDown += MUI_PreviewMouseLeftButtonDown;
                mUI.PreviewMouseMove -= m_panel_PreviewMouseMove;
                mUI.PreviewMouseMove += m_panel_PreviewMouseMove;

                EventManager.RegisterClassHandler(typeof(System.Windows.Controls.Control), 
                    System.Windows.Controls.Control.KeyDownEvent,
                    new KeyEventHandler(MUI_PreviewKeyDown)); 
                mUI.PreviewKeyDown -= MUI_PreviewKeyDown;
                mUI.PreviewKeyDown += MUI_PreviewKeyDown;
            }
            return mUI;
        }

        private void MUI_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (mSelect != null)
                {
                    if (mSelect is entity)
                    {
                        removeEntity(mSelect as entity);
                    }
                    else
                    {
                        removeLinker(mSelect as linker);
                    }
                    var lns = serial();
                    mDatas.data.mData = lns;
                    mDatas.backup();

                    updateUI();
                }
            }
            else if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                mDatas.undo();
                unserial(mDatas.data.mData);
                //getUI().PreviewMouseLeftButtonDown -= MUI_PreviewMouseLeftButtonDown;
                //getUI().PreviewMouseLeftButtonDown += MUI_PreviewMouseLeftButtonDown;
                //getUI().PreviewMouseMove -= m_panel_PreviewMouseMove;
                //getUI().PreviewMouseMove += m_panel_PreviewMouseMove;

                //EventManager.RegisterClassHandler(typeof(System.Windows.Controls.Control),
                //    System.Windows.Controls.Control.KeyDownEvent,
                //    new KeyEventHandler(MUI_PreviewKeyDown));
                //getUI().PreviewKeyDown -= MUI_PreviewKeyDown;
                //getUI().PreviewKeyDown += MUI_PreviewKeyDown;
            }
            else if (e.Key == Key.Y && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                mDatas.redo();
                unserial(mDatas.data.mData);
            }
        }

        private void MUI_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if(Keyboard.IsKeyDown(Key.LeftCtrl) )
                {
                    var pos = Mouse.GetPosition(mUI);
                    addEntity("entity1", (float)pos.X, (float)pos.Y);
                    var lns = serial();
                    mDatas.data.mData = lns;
                    mDatas.backup();
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
