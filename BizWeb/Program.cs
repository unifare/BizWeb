using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UniOrm;
using UniOrm.Startup.Web;

namespace UniNote.WebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.LogInfo("Program", "Program is starting");
            //var host = CreateWebHostBuilder(args).Build();

            UniOrm.Startup.Web.WebSetup.StartApp(args);
            //WebSetup.StartApp(args);
        }
      
    }
}
