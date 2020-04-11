using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
namespace UniOrm.Common
{
    public class JsonConfig : IConfig
    {
        public JToken root = null;
        public void Save(string filepath)
        {
            File.WriteAllText(filepath, root.ToString());
        }

        public string Source { get; set; }

        public JToken getnodevalue(JToken jObject, object tindex)
        {
            JToken jToken = jObject;
            if (tindex is int)
            {
                var ind = (int)tindex;
                jToken = (jObject)[ind];
            }
            if (tindex is string)
            {
                jToken = (jObject)[tindex];
            }
            return jToken;
        }

        public T GetValue<T>(params object[] keyorPaths)
        {
            if (root == null)
            {
                root = JObject.Parse(Source);
            }
            var tem = GetToken(keyorPaths, root);
            var types = typeof(T);
            if (types.IsValueType || types == typeof(string))
            {
                return tem.Value<T>();
            }
            else
            {
                 return tem.ToString().ToObject<T>();
            }
        }

        private JToken GetToken(object[] keyorPaths, JToken tem)
        {

            if (keyorPaths == null || keyorPaths.Length == 0)
            {
                //return default(T);
            }
            else
            {
                //var i = 0;

                foreach (var s in keyorPaths)
                {
                    //if (i == 0)
                    //{
                    //    tem = root[s];
                    //}
                    //else
                    //{
                    tem = tem[s];
                    //}
                    //i++;
                }
            }

            return tem;
        }

        public void Set(object value, params object[] keyorPaths)
        {
            if (root == null)
            {
                root = JToken.Parse(Source);
            }
            if (keyorPaths == null || keyorPaths.Length == 0)
            {
                return;
            }
            var ser = keyorPaths.ToList();
            ser.RemoveAt(keyorPaths.Length - 1);
            var tem = GetToken(ser.ToArray(), root);
            tem.SetValue(keyorPaths[keyorPaths.Length - 1].ToString(), value);
        }
    }
}
