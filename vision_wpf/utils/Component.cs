using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_vision
{
    public abstract class Component
    {
        public virtual void inherit() { }
        public virtual void init() { }
        

        public ComponentContainer container
        {
            get;
            protected set;
        }

        protected Component() : this(null) {}

        protected Component(ComponentContainer cont)
        {
            container = cont;
            if (cont == null)
            {
                container = new ComponentContainer();
            }
        }

        public T addComponent<T>() where T : Component, new()
        {
            return container.addComponent<T>();
        }

        public T getComponent<T>() where T : Component, new()
        {
            return container.getComponent<T>();
        }

        public static T create<T>(ComponentContainer cont) where T : Component, new()
        {
            var ins = new T();
            ins.container = cont;
            ins.inherit();
            if(cont != null)
            {
                cont._container.Add(ins);
            }
            ins.init();
            return ins;
        }
    }

    public class ComponentContainer
    {
        public List<Component> _container = new List<Component>();
        public T addComponent<T>() where T : Component, new()
        {
            for (int i = 0; i < _container.Count; ++i)
            {
                if (_container[i] is T)
                {
                    return _container[i] as T;
                }
            }

            T ins = Component.create<T>(this);
            return ins;
        }

        public T getComponent<T>() where T : Component
        {
            for (int i = 0; i < _container.Count; ++i)
            {
                if (_container[i] is T)
                {
                    return _container[i] as T;
                }
            }
            return null;
        }
    }


}
