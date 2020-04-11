using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.ShopExPlugin.Model
{
    public class OrderBillItem
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string OrderGuid { get; set; }

        public string ProductId { get; set; }

        public double? ItemQuatity { get; set; }

        public decimal? ItemPrice { get; set; }

        public DateTime? AddTime { get; set; }
    }
}
