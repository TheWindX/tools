using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MiniEditor
{
    class PropCom : MComponent
    {
        Func<object> fGetPropObj = null;
        public void refreshProp()
        {
            var o = fGetPropObj();
            Type t = o.GetType();
            if (t.IsClass)
            {
                EditorFuncs.instance().getPropWindow().Children.Clear();
                var fs = t.GetProperties();
                foreach (var f in fs)
                {
                    if (f.PropertyType == typeof(int))
                    {
                        var v = new intField();
                        v.Name = f.Name;
                        v.Val = (int)f.GetValue(o);
                        v.evtValueChanged += () => {
                            f.SetValue(o, v.Val);
                        };
                        EditorFuncs.instance().getPropWindow().Children.Add(v);
                    }
                    else if (f.PropertyType == typeof(Boolean))
                    {
                        var v = new boolField();
                        v.Name = f.Name;
                        v.Val = (bool)f.GetValue(o);
                        v.evtValueChanged += () => {
                            f.SetValue(o, v.Val);
                        };
                        EditorFuncs.instance().getPropWindow().Children.Add(v);
                    }
                    MLogger.info(String.Format("name:{0}, value:{1}", f.Name, f.GetValue(o)));
                }

            }
        }

        
    }
    
    
}
