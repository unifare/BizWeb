using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using UniOrm.Common;

namespace UniOrm.Startup.Web
{
    public class WebStarupAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance<IAuthorizeHelper>(new AuthorizeHelper());
            base.Load(builder);

        }
    }
}
