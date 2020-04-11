using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.ShopExPlugin.Model
{
    public class ProductType
    {
        public int Id { get; set; }

        public string Guid { get; set; }

        public string TypeName { get; set; }
        public string TypeParentID { get; set; }

        public bool? IsShow { get; set; }

        public bool? IsSystem { get; set; }

        public DateTime AddTime { get; set; }

    }
}
