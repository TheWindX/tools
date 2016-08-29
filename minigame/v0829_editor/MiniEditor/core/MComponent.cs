﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    class EditObject
    {
        List<MComponent> mComponents = new List<MComponent>();

        public MComponent addComponent(Type t)
        {
            foreach (var com in mComponents)
            {
                if (com.GetType() == t)
                {
                    throw new Exception("already add same component");
                }
            }
            var c = (MComponent)Activator.CreateInstance(t);
            c.go = this;

            var dps = MComponent.getDependcy(t);
            foreach (var d in dps)
            {
                addComponent(d.com);
            }
            this.mComponents.Add(c);
            return c;
        }

        public T addComponent<T>() where T: MComponent
        {
            return addComponent(typeof(T)) as T;
        }

        public MComponent getComponent(Type t)
        {
            foreach (var com in mComponents)
            {
                if (com.GetType() == t)
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

        public T getOrAddComponent<T>() where T: MComponent
        {
            var t = getComponent<T>();
            if(t == null)
            {
                return addComponent<T>();
            }
            return t;
        }

        public void removeComponent(Type t)
        {
            var com = getComponent(t);

            var dps = MComponent.getDependcy(t);
            foreach (var d in dps)
            {
                removeComponent(d.com);
            }
            if (com != null)
                mComponents.Remove(com);
        }

        public void removeComponent<T>() where T:MComponent
        {
            removeComponent(typeof(T));
        }
    }

    public class MComponent
    {
        internal EditObject go = null;
        internal Action evtAdd = null;
        internal Action evtRemove = null;

        public T addComponent<T>() where T : MComponent
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

        internal static IEnumerable<RequireCom> getDependcy(Type t)
        {
            var attrs = t.GetCustomAttributes(true);
            foreach(var attr in attrs)
            {
                if(attr is RequireCom)
                {
                    yield return attr as RequireCom;
                }
            }
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true,  Inherited = true)]
    public class RequireCom : System.Attribute
    {
        public Type com;

        public RequireCom(Type com)
        {
            this.com = com;
        }
    }

}
