using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    public static class ComponentHelper
    {
        public static Action getDynamicFunc(Object obj, string fname, bool isMember = true, bool isPublic=true)
        {
            MethodInfo mi = null;
            try
            {
                BindingFlags bf = BindingFlags.Default;
                if (isMember)
                    bf = bf | BindingFlags.Instance;
                else
                    bf = bf | BindingFlags.Static;

                if (isPublic)
                    bf = bf | BindingFlags.Public;
                else
                    bf = bf | BindingFlags.NonPublic;

                mi = obj.GetType().GetMethod(fname, bf);
            }
            catch(Exception ex)
            {
                return null;
            }
            if (mi == null) return null;
            return () =>
            {
                try
                {
                    mi.Invoke(obj, null);
                }
                catch(Exception ex)
                {
                    MLogger.error(ex.Message);
                }
            };            
        }

    }
}
