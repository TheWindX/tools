using System;
using System.Reflection;
using System.Windows.Controls;

namespace MiniEditor
{
    class EditorWorld
    {
        static EditorObject mRootObject = createObject(null, "root");
        public static EditorObject getRootEditorObject()
        {
            return mRootObject;
        }

        public static EditorObject getCurrentObj()
        {
            return EditorFuncs.getItemListPage().getCurrentObj();
        }

        public static EditorObject createObject(EditorObject parent, string name)
        {
            var eo = MWorld.createObject(parent, name);
            eo.addComponent<COMEditorObject>();
            return eo;
        }

        public static void removeObject(EditorObject obj)
        {
            MWorld.removeObject(obj);
        }

        public static void addTypeControl(MComponent com, ComponentPanel comPanel)
        {
            var panel = comPanel.mProps;
            panel.Children.Clear();
            Type t = com.GetType();
            if (t.IsClass)
            {
                var ms = t.GetMethods();
                MethodInfo inspector = null;
                foreach (var m in ms)
                {
                    var inspectorProp = m.GetCustomAttribute<CustomInspectorAttribute>();
                    if(inspectorProp != null)
                    {
                        inspector = m;
                        break;
                    }
                }

                if(inspector != null)
                {
                    inspector.Invoke(com, null);
                    //description
                    var fs = t.GetProperties();
                    foreach (var p in fs)
                    {
                        if (p.PropertyType == typeof(string))
                        {
                            var descProp = p.GetCustomAttribute<DescriptionAttribute>();
                            if (descProp != null)
                            {
                                var v = new DiscriptionCtrl();
                                v.text = (string)p.GetValue(com);
                                panel.Children.Add(v);
                            }
                        }
                    }
                }
                else //property to gen inpector
                {
                    var fs = t.GetProperties();
                    foreach (var p in fs)
                    {
                        var editorProp = p.GetCustomAttribute<EditorPropertyAttribute>();
                        if (p.PropertyType == typeof(int))
                        {
                            var v = new intField();
                            v.Lable = p.Name;
                            v.Val = (int)p.GetValue(com);
                            v.evtValueChanged += () => {
                                p.SetValue(com, v.Val);
                            };
                            panel.Children.Add(v);
                            if (editorProp != null)
                                v.IsEnabled = false;

                            Action act = () =>
                            {
                                var checkValue = (int)p.GetValue(com);
                                if(checkValue != v.Val)
                                {
                                    v.Val = checkValue;
                                }
                            };

                            com.evtUpdate += act;
                            com.evtRemove += () =>
                            {
                                com.evtUpdate -= act;
                            };
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
                            if (editorProp != null)
                                v.IsEnabled = false;
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
                            if (editorProp != null)
                                v.IsEnabled = false;

                            Action act = () =>
                            {
                                var checkValue = (double)p.GetValue(com);
                                if (checkValue != v.Val)
                                {
                                    v.Val = checkValue;
                                }
                            };

                            com.evtUpdate += act;
                            com.evtRemove += () =>
                            {
                                com.evtUpdate -= act;
                            };
                        }
                        else if (p.PropertyType == typeof(string))
                        {
                            var descProp = p.GetCustomAttribute<DescriptionAttribute>();
                            if (descProp != null)
                            {//description
                                var v = new DiscriptionCtrl();
                                v.text = (string)p.GetValue(com);
                                panel.Children.Add(v);
                            }
                            else
                            {
                                var v = new stringField();
                                v.Lable = p.Name;
                                v.Val = (string)p.GetValue(com);
                                v.evtValueChanged += () => {
                                    p.SetValue(com, v.Val);
                                };
                                panel.Children.Add(v);
                                if (editorProp != null)
                                    v.IsEnabled = false;
                            }
                        }
                        else if(p.PropertyType == typeof(System.Action))
                        {
                            var v = new UIAction();
                            v.Lable = p.Name;
                            v.Val = (System.Action)p.GetValue(com);
                            panel.Children.Add(v);
                        }
                    }
                }
                //var o = Activator.CreateInstance(t);
                
            }
        }
    }
}
