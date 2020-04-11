using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniOrm.Core
{
    public class SystemlStructureManager
    {
        public ConfigureManager ConfigureManager { get; set; }

        public SystemlStructureManager()
        {
            ConfigureManager = new ConfigureManager();

        }
        //public ConfigureManager Read
    }
}
