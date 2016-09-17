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
using System.Reflection;

namespace MiniEditor
{
    class RepoNode
    {
        public string name
        {
            get;set;
        }

        private RepoBranch mParent = null;
        public RepoBranch parent
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
    }

    class RepoLeaf : RepoNode
    {
        public Type component{get;set;}
    }

    class RepoBranch : RepoNode
    {
        internal List<RepoNode> mChildren = new List<RepoNode>();
        public IEnumerable<RepoNode> children
        {
            get
            {
                return mChildren;
            }
        }

        public RepoBranch addBranch(string branchName)
        {
            var b = new RepoBranch() { name = branchName };
            mChildren.Add(b);
            return b;
        }

        public RepoBranch addOrGetBranch(string branchName)
        {
            var b = mChildren.Find(c=>c.name == branchName);
            if(b == null)
            {
                b = addBranch(branchName);
            }
            return b as RepoBranch;
        }

        public RepoLeaf addLeaf(string leafName, Type com)
        {
            var leaf = new RepoLeaf() { name = leafName, component = com };
            mChildren.Add(leaf);
            return leaf;
        }
    }
    
    class ComponentRepository
    {
        public static RepoBranch root = new RepoBranch() { name = "root" };
        public static void addComponent(Type com)
        {
            if(!typeof(MComponent).IsAssignableFrom(com))
            {
                MLogger.error("{0}不是有效的component", com.Name);
                return;
            }
            var attr = com.GetCustomAttribute<CustomComponentAttribute>();
            var path = attr.path;
            var name = attr.name;
            path = path.Replace('\\', '/');
            var segments = path.Split('/');
            RepoBranch b = root;
            if (string.IsNullOrEmpty(path))
            {
                MLogger.info("{0}没有指定的path", com.Name);
                b.addLeaf(name, com);
                return;
            }

            for(int i = 0; i<segments.Count(); ++i)
            {
                b = b.addOrGetBranch(segments[i]);
            }
            b.addLeaf(name, com);
        }

        public static IEnumerable<RepoNode> subItems
        {
            get
            {
                init();
                return root.children;
            }
        }

        private static RepoLeaf getComponentByNameFromRepoBranch(RepoBranch b, string name)
        {
            foreach(var node in b.children)
            {
                if(node is RepoLeaf)
                {
                    var leaf = node as RepoLeaf;
                    if (leaf.component.Name == name || leaf.component.Name == "COM"+name)
                    {
                        return node as RepoLeaf;
                    }
                }
                else if(node is RepoBranch)
                {
                    var l =  getComponentByNameFromRepoBranch(node as RepoBranch, name);
                    if (l != null) return l;
                }
            }
            return null;
        }

        public static RepoLeaf getComponentByName(string name)
        {
            init();
            return getComponentByNameFromRepoBranch(root, name);
        }

        static List<Type> getAssemblyComponents()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            var ts = myAssembly.GetTypes();

            List<Type> mComponents = new List<Type>();
            foreach (var t in ts)
            {
                if (typeof(MComponent).IsAssignableFrom(t))
                {
                    var attrs = t.GetCustomAttribute<CustomComponentAttribute>();
                    if (attrs != null)
                    {  
                        mComponents.Add(t);
                    }
                }
            }
            return mComponents;
        }

        static bool mInit = false;
        public static void init()
        {
            if (mInit) return;
            else
            {
                var types = getAssemblyComponents();
                foreach (var comType in types)
                {
                    addComponent(comType);
                }
                mInit = true;
            }
        }
    }
}
