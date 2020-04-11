using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace UniOrm.Application
{
    public interface IPool
    {
        Dictionary<string, object> PoolResource { get; set; }
        void MapToPoolResouce(string key, string objectValue);
        //Func<List<object>, PoolResource> PoolResourceMakeActio { get; set; }
        void PoolResourceMakeAction();
    }
}
