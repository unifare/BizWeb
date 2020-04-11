using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UniOrm.Core
{
    public class BaseElement
    {
        public string Name { get; set; }
        public string Uuid { get; set; }

        public XElement SourceElement { get; set; }
        
    }
}
