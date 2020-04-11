using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UniOrm.Core
{
    public class XmlWorker
    {
        public XElement xElement { get; set; }

        public XmlWorker(XElement xelement)
        {
            xElement = xelement;
        }

        public XmlWorker this[string elementName]
        {
            get
            {
                return  new XmlWorker(xElement.Element(elementName));
            }
             
        }
        

        
    }
}
