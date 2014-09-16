using System;
using System.Reflection;

namespace MWS.Helper
{
    public class Singleton<T> where T : class
    {
        // singleton
        private static volatile T _instance;
        private static object syncRoot = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            ConstructorInfo ci = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                            
                            if (ci == null) 
                            { 
                                throw new InvalidOperationException("Class must contain a private constructor"); 
                            }

                            _instance = (T)ci.Invoke(null);
                        }
                    }
                }

                return _instance;
            }
        }
    }
}
