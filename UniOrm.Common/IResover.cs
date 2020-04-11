using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common
{
    public interface IResover
    {
        object Rtor { get; }
        object Resolve(Type type);
        T Resolve<T>();
    }
     

}
