using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Application
{
    public class HttpPoolbase:IPool
    {
        public Dictionary<string ,object> PoolResource { get; set; }
        public void MapToPoolResouce(string key ,string objectValue )
        {
            if(!PoolResource.ContainsKey(key))
            {
                PoolResource[key] = objectValue;
            }
        }

        public void PoolResourceMakeAction()
        {

        }
    }
}
