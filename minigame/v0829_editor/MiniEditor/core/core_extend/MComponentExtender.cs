using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MiniEditor
{
    static class MComponentExtender
    {
        public static XmlElement toXML(this MComponent component, XmlDocument doc)
        {
            var comType = component.GetType();
            var attr = comType.GetCustomAttribute<CustomComponentAttribute>();
            if (attr == null)
            {
                MLogger.error("component type of {0} is not a custom component", comType.Name);
                return null;
            }
            var r = doc.CreateElement(comType.Name.Replace("COM", ""));
            var props = comType.GetProperties();
            foreach (var prop in props)
            {
                var v = prop.GetValue(component);
                var a = doc.CreateAttribute(prop.Name);
                a.Value = v.ToString();
                r.Attributes.Append(a);
            }
            return r;
        }

        public static MComponent componentFromXML(EditorObject obj, XmlElement elem)
        {
            var name = elem.Name;
            var leaf = ComponentRepository.getComponentByName(name);
            if (leaf == null)
            {
                MLogger.error("cannot find component name of {0}", name);
                return null;
            }
            var props = leaf.component.GetProperties();
            var instances = obj.addComponent(leaf.component);
            var instance = instances.Last();
            foreach (var prop in props)
            {
                var value = elem.GetAttribute(prop.Name);
                if(prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(instance, Convert.ToBoolean(value));
                }
                else if (prop.PropertyType == typeof(int))
                {
                    prop.SetValue(instance, Convert.ToInt32(value));
                }
                else if (prop.PropertyType == typeof(double))
                {
                    Double d = double.Parse(value, CultureInfo.InvariantCulture);
                    prop.SetValue(instance, d);
                }
                else if (prop.PropertyType == typeof(string))
                {
                    if(prop.GetCustomAttribute<DescriptionAttribute>() == null)
                    {
                        prop.SetValue(instance, value);
                    }
                }
            }
            return instance;
        }

        public static CustomComponentAttribute getAttr(this MComponent component)
        {
            return component.GetType().GetCustomAttribute<CustomComponentAttribute>();
        }


        public static EditorObject editorObjectFromXML(EditorObject parent, XmlElement elem)
        {
            EditorObject obj = null;
            if (elem.Name == "root")
            {
                obj = EditorWorld.getRootEditorObject();
            }
            else if(elem.Name != "node")
            {
                var name = elem.GetAttribute("name");
                obj = EditorWorld.createObject(parent, name);
            }
            else
            {
                var name = elem.GetAttribute("name");
                obj = EditorWorld.createObject(parent, name);
            }

            //children
            foreach (XmlElement sub in elem.ChildNodes)
            {   
                if(sub.Name == "children")
                {
                    //children = EditorWorld.createObject(obj, "children");
                    foreach (XmlElement childElement in sub.ChildNodes)
                    {
                        editorObjectFromXML(obj, childElement);
                    }
                }
                else
                {
                    var com = componentFromXML(obj, sub);
                }
            }
            return obj;
        }

        public static XmlElement toXML(this EditorObject obj, XmlDocument doc)
        {
            var coms = obj.components;
            var r = doc.CreateElement("node");
            foreach (var com in coms)
            {
                var elem = com.toXML(doc);
                r.AppendChild(elem);
            }
            var children = doc.CreateElement("children");
            foreach (var subObj in obj.children)
            {
                children.AppendChild(subObj.toXML(doc));
            }
            if(children.ChildNodes.Count != 0)
                r.AppendChild(children);
            return r;
            //MComponent mainCom = null;
            //List<MComponent> sortedComs = new List<MComponent>();
            //List<MComponent> noMainComs = new List<MComponent>();
            //foreach (var com in coms)
            //{
            //    if (com.getAttr().isMain)
            //    {
            //        mainCom = com;
            //    }
            //    else
            //    {
            //        noMainComs.Add(com);
            //    }
            //}
            //if (mainCom != null)
            //{
            //    sortedComs.Add(mainCom);
            //}
            //sortedComs.AddRange(noMainComs);
            //mainCom = sortedComs[0];
            //var restCom = sortedComs.Skip(1);
            //var r = mainCom.toXML(doc);
            //foreach (var com in restCom)
            //{
            //    r.AppendChild(com.toXML(doc));
            //}

            //if (obj.children.Count() != 0)
            //{
            //    var children = doc.CreateElement("children");
            //    foreach (var subObj in obj.children)
            //    {
            //        children.AppendChild(subObj.toXML(doc));
            //    }
            //    r.AppendChild(children);
            //}
            //return r;
        }

        public static string toString(this EditorObject obj)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            var t = obj.toXML(doc);
            doc.AppendChild(t);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter, settings))
            {
                doc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public static void printOBJ(this EditorObject obj)
        {
            MLogger.info(obj.toString());
        }
    }
}
