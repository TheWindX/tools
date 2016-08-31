using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MiniEditor
{
    class EditorWorld
    {
        public static EditObject createObject(EditObject parent, string name)
        {
            var eo = MWorld.createObject(parent, name);
            eo.addComponent<editorCOM>();
            return eo;
        }

        public static void removeObject(EditObject obj)
        {
            MWorld.removeObject(obj);
        }
        
        public static List<Type> getAssemblyModules()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            var ts = myAssembly.GetTypes();

            List<Type> mComponents = new List<Type>();
            foreach (var t in ts)
            {
                if (typeof(MComponent).IsAssignableFrom(t))
                {
                    var attrs = t.GetCustomAttribute<ComponentCustomAttribute>();
                    if (attrs != null)
                    {
                        //MModule instance = (MModule)Activator.CreateInstance(t);
                        mComponents.Add(t);
                    }
                }
            }
            return mComponents;
        }

        public static void addTypeControl(MComponent com, Panel panel)
        {
            panel.Children.Clear();
            Type t = com.GetType();
            if (t.IsClass)
            {
                //var o = Activator.CreateInstance(t);
                var fs = t.GetProperties();
                foreach (var p in fs)
                {
                    if (p.PropertyType == typeof(int))
                    {
                        var v = new intField();
                        v.Lable = p.Name;
                        v.Val = (int)p.GetValue(com);
                        v.evtValueChanged += () => {
                            p.SetValue(com, v.Val);
                        };
                        panel.Children.Add(v);
                        //EditorFuncs.instance().getPropWindow().Children.Add(v);
                    }
                    else if (p.PropertyType == typeof(bool))
                    {
                        var v = new boolField();
                        v.Lable = p.Name;
                        v.Val = (bool)p.GetValue(com);
                        v.evtValueChanged += () => {
                            p.SetValue(com, v.Val);
                        };
                        panel.Children.Add(v);
                    }
                    else if (p.PropertyType == typeof(double))
                    {
                        var v = new doubleField();
                        v.Lable = p.Name;
                        v.Val = (double)p.GetValue(com);
                        v.evtValueChanged += () => {
                            p.SetValue(com, v.Val);
                        };
                        panel.Children.Add(v);
                    }
                    else if (p.PropertyType == typeof(string))
                    {
                        var v = new stringField();
                        v.Lable = p.Name;
                        v.Val = (string)p.GetValue(com);
                        v.evtValueChanged += () => {
                            p.SetValue(com, v.Val);
                        };
                        panel.Children.Add(v);
                    }
                }
            }
        }
    }
}
