using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using UniOrm.Model;
using Microsoft.Extensions.DependencyInjection;

namespace UniOrm.Common
{
    public class WebSitHelper
    {
        IHostingEnvironment HostingEnvironment { get; set; }
        IModule Module { get; set; }
        AConFlowStep Step { get; set; }
        public WebSitHelper(IModule module, AConFlowStep step)
        {
            APPCommon.ApplicationServices.GetService<IHostingEnvironment>();
            Module = module;
            Step = step;
        }

        public string wwwroot
        {

            get
            {
                return HostingEnvironment.WebRootPath;
            }
        }

        public string webroot
        {

            get
            {
                return HostingEnvironment.WebRootPath;
            }
        }

        //public string mname
        //{

        //    get
        //    {
        //        if(Module!=null)
        //        {
        //            return "";
        //        }
        //        return Module.mo
        //    }
        //}
    }
}
