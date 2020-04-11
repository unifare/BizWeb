using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace UniOrm.DepenceCore
{
    public class MaterialSet
    {
        public List<Dictionary<string, object>> Materials { get; set; }
        public MaterialSet()
        {
            Materials = new List<Dictionary<string, object>>();
        }
        public JArray JArray { get; set; }
        public T GetObject<T>(string path, string asType, string assemblyname, int index = 0)
        {
            string[] allpaths = path.Split('.');
            object temobj = null;
            if (allpaths.Length > 1)
            {
                foreach (var s in allpaths)
                {
                    //Materials[index][path]
                }
            }
            else
            {
                temobj = Materials[index][path];
            }

            if (temobj != null)
            {
                //Type s = new a
                return (T) Convert.ChangeType(temobj,typeof(T));
            }
            return default(T);
        }

    }
}
