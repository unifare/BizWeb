using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Text; 
namespace UniOrm
{
    public static class MagicExtension
    {
        public static object BackToInst(object obj)
        {
            if (obj == null)
                return null;
            var ww = obj as PrivateReflectionDynamicObjectBase;
            if( ww==null)
            { 
                return obj;
            }
            return ww.Instance;
        }

        public static object GetProp(this object obj , string PropName)
        {
            if (obj == null)
                return null;
           
            var pi= obj.GetType().GetProperty(PropName);
            if(pi==null)
            {
                return null;
            }
            else
            {
                return pi.GetValue(obj);
            }
           
        }

        public static object Invoke(this object obj, string methodName, object[] args = null)
        {
            if (obj == null)
                return null;
            var pi = obj.GetType().GetMethod(methodName);
            if (pi == null)
            {
                return null;
            }
            else
            {
                return pi.Invoke(obj, args);
            }
        }
        public static object Invoke(this Type type, object obj, string methodName, object[] args = null)
        {
            if (obj == null)
                return null;
            var pi = type.GetMethod(methodName);
            if (pi == null)
            {
                return null;
            }
            else
            {
                return pi.Invoke(obj, args);
            }
        }

    }
}
