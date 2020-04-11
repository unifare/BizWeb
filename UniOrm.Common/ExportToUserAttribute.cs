using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common
{
    [AttributeUsage(AttributeTargets.All)]
    public  class ExportTypeAttribute : Attribute
    {
        private readonly string _name;

        public string Name
        {
            get { return _name; }
        }
        public ExportTypeAttribute( )
        { 
        }
        public ExportTypeAttribute(string name)
        {
            _name = name;
        }
    }

    //[AttributeUsage(AttributeTargets.Method)]
    //public class ExportTypeAttribute : Attribute
    //{
    //    private readonly string _name;

    //    public string Name
    //    {
    //        get { return _name; }
    //    }
    //    public ExportTypeAttribute()
    //    {
    //    }
    //    public ExportTypeAttribute(string name)
    //    {
    //        _name = name;
    //    }
    //}
}
