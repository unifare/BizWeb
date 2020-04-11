using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UniOrm.Core
{
    public class ConfigureManager
    {
        public List<Layout> Layouts { get; set; }
        public Dictionary<string, XElement> XmlConfigers { get; set; }

        public ConfigureManager()
        {
            Layouts = new List<Layout>();
            XmlConfigers = new Dictionary<string, XElement>();
        }

        public void AddConfigWorker(string workerName, string filePath)
        {
            XElement xele = XElement.Load(filePath);
            XmlConfigers.Add(workerName, xele);
        }
        //public T XMLRead<T>(string workerName,string Pathe)
        //{
        //    xmlConfigers[workerName].Document.Root.Descendants()
        //     XElement xele1 = xele.Element("Item");
        //    Console.Write(xele1.Value.Trim());
        //}
    }
}
