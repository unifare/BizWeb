using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common.Core
{
    public interface IStartUp
    {


        IConfiguration Configuration { get; }

        void ConfigureContainer(ContainerBuilder containerBuilder);

        // This method gets called by the runtime. Use this method to add services to the container.
        void ConfigureServices(IServiceCollection services);

        void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime);
    }
}
