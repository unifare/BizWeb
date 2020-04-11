using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UniOrm.Common
{
    public static class ConfigBuilder
    {
        //public static IConfig Crate(  string path)
        //{

        //}
        public static IConfig AddFile(this IConfig config, string path)
        {
            config.Source = File.ReadAllText(path);
            return config;
        }
    }
}
