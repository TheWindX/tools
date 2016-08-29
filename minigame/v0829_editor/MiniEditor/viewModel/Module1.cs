using MiniEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor.viewModel
{
    class testType
    {
        int m_asdf = 11;
        public int asdf
        {
            get
            {
                return m_asdf;
            }
            set
            {
                m_asdf = value;
                MLogger.info("set asdf:{0}", m_asdf);
            }
        }
        bool m_bbbb = false;
        public bool bbbb
        {
            get
            {
                return m_bbbb;
            }
            set
            {
                m_bbbb = value;
                MLogger.info("set bbbb:{0}", m_bbbb);
            }
        }
    }
    [MiniEditor.ModuleInstance(1)]
    class Module1 : MModule
    {
        public void onExit()
        {
            
        }

        public void onInit()
        {
            MTimer.get().setTimeout(t =>
            {
                var tt = new testType();
                addTypeControl(tt);
            }, 1200);
        }

        public void onUpdate()
        {
            
        }

        void addTypeControl(object o)
        {
            Type t = o.GetType();
            if(t.IsClass)
            {
                //var o = Activator.CreateInstance(t);
                EditorFuncs.instance().getPropWindow().Children.Clear();
                var fs = t.GetProperties();
                foreach (var p in fs)
                {
                    if(p.PropertyType == typeof(int))
                    {
                        var v = new intField();
                        v.Name = p.Name;
                        v.Val = (int)p.GetValue(o);
                        v.evtValueChanged += () => {
                            p.SetValue(o, v.Val);
                        };
                        EditorFuncs.instance().getPropWindow().Children.Add(v);
                    }
                    else if(p.PropertyType == typeof(Boolean))
                    {
                        var v = new boolField();
                        v.Name = p.Name;
                        v.Val = (bool)p.GetValue(o);
                        v.evtValueChanged += () => {
                            p.SetValue(o, v.Val);
                        };
                        EditorFuncs.instance().getPropWindow().Children.Add(v);
                    }
                    MLogger.info(String.Format("name:{0}, value:{1}", p.Name, p.GetValue(o)));
                }
                
            }
        }


    }
}
