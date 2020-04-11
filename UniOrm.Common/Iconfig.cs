using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm 
{
    public interface IConfig
    {
        void Save(string filepath);
        string Source { get; set; }
        T GetValue<T>(params object[]   keyorPaths);


        void Set(object value, params object[] keyorPaths);


    }
}
