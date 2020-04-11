using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.DistributeSystemPlugin.Model
{
    public  class ProxyPolicy
    { 
       
        public int Id { get; set; }
        public ProxyPolicy()
        {

        }
        public string ProductGuid { get; set; }
        public string Guid { get; set; }
       
        public decimal? RateForPrice { get; set; }
      
        public decimal? PriceForBill { get; set; }
     
        public DateTime? CreateDate { get; set; }
        
        public int? TaocanId { get; set; }
      
        public int? RateType { get; set; }
      
        public bool IsDisabled { get; set; }
       
        public int? ProxyLevel { get; set; }
        
        public int? ProxyType { get; set; }

     

    }

    
}
