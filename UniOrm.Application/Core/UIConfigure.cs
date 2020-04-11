using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UniOrm.Core
{
    public class  Configure
    {
        public XElement xElement { get; set; }
        public List<Layout> Layouts { get; set; }
    }
}
