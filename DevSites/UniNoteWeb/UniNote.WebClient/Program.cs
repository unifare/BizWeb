using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
            CreateWebHostBuilder(args).Build().Run();
            //WebSetup.StartApp(args);
        }
        private static string[] ScanBack(string dlldir)
        {
            return Directory.GetFiles(dlldir, "*.json");
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
          var webHostBuilder =  WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {        
                    builder.AddJsonFile( "config/System.json");
                    var alljsons = ScanBack(AppDomain.CurrentDomain.BaseDirectory);
                    foreach (var json in alljsons)
                    {
                        builder.AddJsonFile(json);
                    }
          
                })
                .UseStartup<Startup>();
            return webHostBuilder;
        }
    }
}
