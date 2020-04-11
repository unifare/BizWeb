using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.DistributeSystemPlugin.Model
{
    public  class ProxyFeeDetail
    {
       
        public int Id { get; set; }
        public ProxyFeeDetail()
        {

        }
     
        public string OrderID { get; set; }
      
        public string Ujid { get; set; }
     
        public decimal? TotalPrice { get; set; }
      
        public decimal? CountedFee { get; set; }
      
        public bool  IsChecked { get; set; }
      
        public bool  IsCancel { get; set; }
      
        public DateTime? ApplyTime { get; set; }
       
        public DateTime? CreateTime { get; set; }
        
        public DateTime? CheckedTime { get; set; }
       
        public string SalesCode { get; set; }
       
        public string ApplyID { get; set; }
       
        public decimal? CheckedoutFee { get; set; }
       
        public decimal? UnCheckedoutFee { get; set; }
        
        public int? ProxyType { get; set; }
         
        public int? ProxyLevel { get; set; }
       
        public int? RateType { get; set; }
      
        public decimal? RateForPrice { get; set; }
      
        public decimal? PriceForBill { get; set; }
     
        public bool IsInFronzen { get; set; }
       
        public bool? IsRefonded { get; set; }
      
        public DateTime? UpdateTime { get; set; }
       
        public int? ProxyFrozenDays { get; set; }
      
        public DateTime? OrderTime { get; set; }
        
        public DateTime? PayedTime { get; set; }
        
        public string LockFanProxySalesCode { get; set; }
       
        public string FirstSalescode { get; set; }
    }
}
