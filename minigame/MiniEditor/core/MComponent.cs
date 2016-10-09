/*
 * author: xiaofeng.li
 * mail: 453588006@qq.com
 * desc: 
 * */
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniEditor
{
    public class MObject
    {
        List<MComponent> mComponents = new List<MComponent>();
        List<MObject> mChildren = new List<MObject>();
        MObject mParent = null;

        public string name
        {
            get;
            set;
        }

        public IEnumerable<MObject> children
        {
            get
            {
                return mChildren;
            }
        }

        public MObject parent
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

        public MObject preview
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

        public MObject next
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
                    coms.Add(com);
                    return coms;
                }
            }
            var c = (MComponent)Activator.CreateInstance(t);
            c.eo = this;
            try
            {
                c.editorAwake(); //todo, 是否加到队列
            }
            catch(Exception ex)
            {
                MLogger.error(ex.ToString());
            }

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
            {
                try
                {
                    com.editorSleep(); //todo, 是否加到队列
                }
                catch (Exception ex)
                {
                    MLogger.error(ex.ToString());
                }
                mComponents.Remove(com);
            }   
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
        internal MObject eo = null;

        //add component或enable时回调
        public virtual void editorAwake()
        {
            MLogger.info("editorAwake: {0}", GetType().Name);   
        }

        //remove component或disable时回调
        public virtual void editorSleep()
        {
            MLogger.info("editorSleep: {0}", GetType().Name);
        }

        //进入editor object时回调
        public virtual void editorInit()
        {
            //MLogger.info("editorInit: {0}", GetType().Name);
        }

        //当前editor object时每祯循环回调
        public virtual void editorUpdate()
        {
            //MLogger.info("editorUpdate: {0}", GetType().Name);
        }

        //离开editor object时回调
        public virtual void editorExit()
        {
            //MLogger.info("editorExit: {0}", GetType().Name);
        }

        public MObject getObject()
        {
            return eo;
        }

        public IEnumerable<MComponent> addComponent<T>() where T : MComponent
        {
            return eo.addComponent<T>();
        }

        public T getComponent<T>() where T : MComponent
        {
            return eo.getComponent<T>();
        }

        public void removeComponent<T>() where T : MComponent
        {
            eo.removeComponent<T>();
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
