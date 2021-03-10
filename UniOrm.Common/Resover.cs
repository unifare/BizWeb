
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
namespace UniOrm.Common
{
    public class Resover
    {
        private static ContainerBuilder _container = new ContainerBuilder();
        public ContainerBuilder Container
        {
            set { _container = value; }
            get { return _container; }
        }
        public ILifetimeScope Resovertot
        {
            get; set;
        }

    }


}
