
namespace ns_vision.ns_utils
{
    public class Singleton<T> where T : new()
    {
        private static readonly object _lock = new object();
        private static T instance;

        protected Singleton()
        {
            System.Diagnostics.Debug.Assert(instance == null);
        }

        public static bool Exists
        {
            get
            {
                return instance != null;
            }
        }

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                        }
                    }
                }
                return instance;
            }
        }
    }
}

