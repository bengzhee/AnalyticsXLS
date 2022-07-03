using System.Reflection;

namespace QuantLibrary
{
    public sealed class ObjectCache
    {
        private static ObjectCache instance = null;
        private static readonly object padlock = new object();
        ObjectCache() { }

        public static ObjectCache Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ObjectCache();
                    }
                    return instance;
                }
            }
        }
        public Dictionary<Guid, dynamic> ObjCache = new Dictionary<Guid, dynamic>();
    }

    public sealed class Bag
    {
        public static void CreateObject(Guid uid, object classInstance) 
        {
            ObjectCache cache = ObjectCache.Instance;
            cache.ObjCache.Add(uid, classInstance);
        }

        public static object QueryObject(Guid uid, string methodName, object arg)
        {
            ObjectCache cache = ObjectCache.Instance;
            object obj = cache.ObjCache[uid];
            Type type = obj.GetType();
            MethodInfo methodInfo = type.GetMethod(methodName);
            object[] invokeArg;
            if (methodInfo.GetParameters()[0].ParameterType == typeof(System.DateTime))
                invokeArg = new object[] { DateTime.FromOADate((double)arg) };
            else
                invokeArg = new object[] { arg };
            return methodInfo.Invoke(obj, invokeArg);

        }
    }
        
}
