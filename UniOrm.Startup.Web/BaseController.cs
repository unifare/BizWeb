using Fasterflect;
using Microsoft.AspNetCore.Mvc;
using System;
using UniOrm.Application;

namespace UniOrm.Startup.Web
{
    public class BaseController : Controller
    {
        IGodWorker GodMaker;
        public BaseController(IGodWorker godMaker )
        {
            GodMaker = godMaker;

            //SuperManager.GetModule(ModuleName.Security).CallMethod("")
        }
    }
}
