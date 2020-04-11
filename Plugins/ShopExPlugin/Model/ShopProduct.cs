using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.ShopExPlugin.Model
{
    public class ShopProduct
    {
        public long Id { get; set; }
        public string Guid { get; set; }

        public string  ShopGuid { get; set; }

        public string ProductTitle { get; set; }

        public string ProductName { get; set; } 

        public bool ProductTypeID { get; set; }

        public string PhoneNumber { get; set; }

        public string Telephone { get; set; }

        public string PrdouctContent { get; set; }

        public decimal? Price { get; set; }

        public DateTime AddTime { get; set; }
    }
}
