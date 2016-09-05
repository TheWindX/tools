/*
 * author: xiaofeng.li
 * mail: 453588006@qq.com
 * desc: 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    public class EditorObject
    {
        List<MComponent> mComponents = new List<MComponent>();
        List<EditorObject> mChildren = new List<EditorObject>();
        EditorObject mParent = null;

        public string name
        {
            get;
            set;
        }

        public IEnumerable<EditorObject> children
        {
            get
            {
                return mChildren;
            }
        }

        public EditorObject parent
        {
            get
            {
                return mParent;
            }
            set
            {
                if (mParent != null)
                {
                    mParent.mChildren.Remove(this);
                }
                mParent = value;
                if (parent == null)
                {
                    return;
                }
                mParent.mChildren.Add(this);
            }
        }

        public EditorObject preview
        {
            get
            {
                if (mParent == null) return this;
                int idx = mParent.mChildren.IndexOf(this);
                idx--;
                if (idx < 0) idx = mParent.mChildren.Count-1;
                return mParent.mChildren.ElementAt(idx);
            }
        }

        public EditorObject next
        {
            get
            {
                if (mParent == null) return this;
                int idx = mParent.mChildren.IndexOf(this);
                idx++;
                if (idx >= mParent.mChildren.Count) idx = 0;
                return mParent.mChildren.ElementAt(idx);
            }
        }


        public IEnumerable<MComponent> addComponent(Type t)
        {
            List<MComponent> coms = new List<MComponent>();
            foreach (var com in mComponents)
            {
                if (t.IsAssignableFrom(com.GetType()))
                {
                    return coms;
                }
            }
            var c = (MComponent)Activator.CreateInstance(t);
            c.go = this;

            var dps = MComponent.getDependcy(t);
            foreach (var d in dps)
            {
                coms.AddRange(addComponent(d.com));
            }
            this.mComponents.Add(c);
            coms.Add(c);
            return coms;
        }

        public IEnumerable<MComponent> addComponent<T>() where T: MComponent
        {
            return addComponent(typeof(T));
        }

        public MComponent getComponent(Type t)
        {
            foreach (var com in mComponents)
            {
                if (t.IsAssignableFrom(com.GetType()))
                {
                    return com;
                }
            }
            return null;
        }

        public T getComponent<T>() where T:MComponent
        {
            return getComponent(typeof(T)) as T;
        }

        public IEnumerable<MComponent> removeComponent(Type t)
        {
            List<MComponent> coms = new List<MComponent>();
            var com = getComponent(t);
            if (com == null) return coms;

            var dps = MComponent.getDependcy(t);
            foreach (var d in dps)
            {
                coms.AddRange(removeComponent(d.com));
            }
            if (com != null)
                mComponents.Remove(com);
            coms.Add(com);
            return coms;
        }

        public IEnumerable<MComponent> removeComponent(MComponent com)
        {
            return removeComponent(com.GetType());
        }

        public IEnumerable<MComponent> removeComponent<T>() where T:MComponent
        {
            return removeComponent(typeof(T));
        }

        public IEnumerable<MComponent> components
        {
            get
            {
                return mComponents;
            }
        }
    }

    public class MComponent
    {
        internal EditorObject go = null;
        internal Action evtInit = null;
        internal Action evtUpdate = null;
        internal Action evtRemove = null;

        public virtual void editorInit()
        {

            //MLogger.info("editorInit: {0}", GetType().Name);
        }

        public virtual void editorUpdate()
        {
            //MLogger.info("editorUpdate: {0}", GetType().Name);
        }

        public virtual void editorExit()
        {
            //MLogger.info("editorExit: {0}", GetType().Name);
        }

        public EditorObject getEditorObject()
        {
            return go;
        }

        public IEnumerable<MComponent> addComponent<T>() where T : MComponent
        {
            return go.addComponent<T>();
        }

        public T getComponent<T>() where T : MComponent
        {
            return go.getComponent<T>();
        }

        public void removeComponent<T>() where T : MComponent
        {
            go.removeComponent<T>();
        }

        internal static IEnumerable<RequireComAttribute> getDependcy(Type t)
        {
            var attrs = t.GetCustomAttributes(true);
            foreach(var attr in attrs)
            {
                if(attr is RequireComAttribute)
                {
                    yield return attr as RequireComAttribute;
                }
            }
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true,  Inherited = true)]
    public class RequireComAttribute : System.Attribute
    {
        public Type com;

        public RequireComAttribute(Type com)
        {
            this.com = com;
        }
    }

}
