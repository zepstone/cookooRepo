using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Origin.Complex
{
    public static class OrgComplex
    {

        static private Dictionary<string, object> ComplexObjects =
            new Dictionary<string, object>();

        static public void AddObject<T>(T obj) {
            ComplexObjects.Add(typeof(T).ToString(), obj);
        }

        static public void AddObject<T>(string key, T obj)
        {
            ComplexObjects.Add(key, obj);
        }

        static public T GetObject<T>(string key)
        {
            return ((T)(ComplexObjects[key]));
        }

        static public T GetObject<T>() {
            return ((T)(ComplexObjects[typeof(T).ToString()]));
        }
    }
}
