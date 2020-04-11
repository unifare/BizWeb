using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.ShopExPlugin.Model
{
    public class PhotoProduct
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string PhotoShoperName { get; set; }
        public string Place { get; set; }
        public string PhoteDate { get; set; }
        public decimal?  Price { get; set; }

        public string Unit { get; set; }

        public string PhoteDescribe { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }
        public string ContractPhone { get; set; }
        public DateTime? AddTime { get; set; }
    }
}
