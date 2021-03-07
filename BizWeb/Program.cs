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
            UniOrm.ApplicationManager.Load().Start(args, CreateWebHostBuilder);
            
            //WebSetup.StartApp(args);
        }
        private static string[] ScanBack(string dlldir)
        {
            return Directory.GetFiles(dlldir, "*.json");
        }
        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {

            var webHostBuilder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //这里是Autofac的引用声明
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder .ConfigureAppConfiguration(builder =>
                   {
                       builder.AddJsonFile("config/System.json");
                       var alljsons = ScanBack(AppDomain.CurrentDomain.BaseDirectory);
                       foreach (var json in alljsons)
                       {
                           builder.AddJsonFile(json);
                       }

                   }).UseStartup<Startup>();
               });
            
            return webHostBuilder;
        }
    }
}
