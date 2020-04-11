using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json; 
using System.IO;
using UniOrm.Core; 

namespace UniOrm.Application
{
    public class AConStateAdaper
    {
        public static SystemlStructureManager SystemlStructureManager = new SystemlStructureManager();
        public static List<Section> Sections = new List<Section>();
        public static List<Layout> Layouts = new List<Layout>();
        public static List<AConPage> AConPages = new List<AConPage>();
        public static List<AConModule> ConModule = new List<AConModule>();
        private static string rootPath = AppDomain.CurrentDomain.BaseDirectory;
        public void Init()
        {
            Sections.Clear();

            SystemlStructureManager.ConfigureManager.AddConfigWorker("ui", Path.Combine( rootPath,"~/config/uiconfigure.xml"));
            var uiconfig = SystemlStructureManager.ConfigureManager.XmlConfigers["ui"];

            var allsections = uiconfig.Elements("Sections").Elements("Section");
            foreach (var secxml in allsections)
            {
                var sec = new Section();
                sec.Name = secxml.Attribute("Name").Value;
                sec.Uuid = secxml.Attribute("Uuid").Value;
                //sec.DisplayText = secxml.Attribute("DisplayText").Value;
                //sec.Href = secxml.Attribute("Href").Value;
                //sec.Authorize = (AConAuthorize)Enum.Parse( typeof(AConAuthorize), secxml.Attribute("Authorize").Value); 

                sec.SourceElement = secxml;
                Sections.Add(sec);
            }
            Logger.LogInfo("AConStateAdaper", "Init ->  load Section xml complete. ");

            var layouts = uiconfig.Elements("Sections").Elements("Layout");
            foreach (var secxml in layouts)
            {
                var sec = new Layout();
                sec.Name = secxml.Attribute("Name").Value;
                sec.Uuid = secxml.Attribute("Uuid").Value;
                sec.SourceElement = secxml;

                //sec.Sections = new List<Section>();
                //var pageCheckPoint = secxml.Descendants("Section");
                //foreach (var checkpoint in pageCheckPoint)
                //{
                //    if (checkpoint.Attribute("Name")==)
                //    var p = new PageCheckPoint();
                //    p.Name = checkpoint.Attribute("Name").Value;
                //    p.Uuid = checkpoint.Attribute("Uuid").Value;
                //    p.IsEnable = Convert.ToBoolean(checkpoint.Attribute("IsEnable").Value);
                //    sec.PageCheckPoints.Add(p);
                //}

                //sec.SourceElement = secxml;
                Layouts.Add(sec);
            }
            Logger.LogInfo("AConStateAdaper", "Init ->  load Layout xml complete. ");

            var pages = uiconfig.Descendants("Page");
            foreach (var secxml in pages)
            {
                var sec = new AConPage();
                sec.Name = secxml.Attribute("Name").Value;
                sec.Uuid = secxml.Attribute("Uuid").Value;
                sec.PageCheckPoints = new List<PageCheckPoint>();
                var pageCheckPoint = secxml.Descendants("CheckRightPoint");
                foreach (var checkpoint in pageCheckPoint)
                {
                    var p = new PageCheckPoint();
                    p.Name = checkpoint.Attribute("Name").Value;
                    p.Uuid = checkpoint.Attribute("Uuid").Value;
                    p.IsEnable = Convert.ToBoolean(checkpoint.Attribute("IsEnable").Value);
                    sec.PageCheckPoints.Add(p);
                }

                sec.SourceElement = secxml;
                AConPages.Add(sec);
            }
            Logger.LogInfo("AConStateAdaper", "Init ->  load Page xml complete. ");
           
            SystemlStructureManager.ConfigureManager.AddConfigWorker("modules", Path.Combine(rootPath,  "~/config/ModulesConfigure.xml"));
            var modules = SystemlStructureManager.ConfigureManager.XmlConfigers["modules"].Descendants("Module");
            foreach (var secxml in modules)
            {
                var sec = new AConModule();
                sec.Name = secxml.Attribute("Name").Value;
                sec.Uuid = secxml.Attribute("Uuid").Value;
                sec.AssemblyPath = secxml.Attribute("AssemblyPath").Value;
                if (!string.IsNullOrEmpty(sec.AssemblyPath))
                {
                    sec.Assembly = System.Reflection.Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory+"\\bin\\"+ sec.AssemblyPath);
                    //NHSessionFactory.AddMapAssembley(sec.Assembly);
                }
                sec.SourceElement = secxml;
                ConModule.Add(sec);
            }
            Logger.LogInfo("AConStateAdaper", "Init ->  load Module xml complete. ");
        }
        public object InterMessage(string moduleName, string requestName, params object[] parameters)
        {
            var requestModule = ConModule.FirstOrDefault(p => p.Name == moduleName);
            var mothodsXml = requestModule.SourceElement.Descendants("Method");
            var mothodElement = mothodsXml.FirstOrDefault(p => p.Attribute("AliName").Value == requestName);
            var classElement = mothodElement.Parent.Parent;
            var classname = classElement.Attribute("Namespace").Value + "." + classElement.Attribute("Name").Value;
             var assemblyInfo = System.Reflection.Assembly.Load(classElement.Parent.Parent.Attribute("AssemblyName").Value);
            var ctype = requestModule.Assembly.GetType(classname, false, true);
            var ProporityTypestr = mothodElement.Attribute("ProporityType").Value;
            var bindingFlag = BindingFlags.Public;
            if (ProporityTypestr == "Static")
            {
                bindingFlag = bindingFlag | BindingFlags.Static;
            }
            //PatientGuildSystem.ModuleBLL.GetBeidaSchedulePatientsByDate
            var mo = ctype.GetMethod(mothodElement.Attribute("Name").Value, bindingFlag);
            var reobj = mo.Invoke(null, parameters);
            var outresultJsonstr = mothodElement.Element("OutResult").Value;
            var outresultJson = JObject.Parse(outresultJsonstr);
            var typestr = outresultJson["type"].Value<string>();
            switch (typestr)
            {
                case "JsonArray":
                    reobj = JsonConvert.SerializeObject(reobj);
                    break;
            }
            return reobj;
        }


    }
}