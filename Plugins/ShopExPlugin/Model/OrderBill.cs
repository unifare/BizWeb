using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.ShopExPlugin.Model
{
    public class OrderBill
    {
        public int Id { get; set; }
        public string Guid { get; set; } 
      
        public string OrderID { get; set; }
         
        public string CostomerName { get; set; }
      
        public string CostomerHandtel { get; set; }
        
        public string CostomerEmail { get; set; }
       
        /// <summary>
        /// 推荐人微信id
        /// </summary>
        public string ReferorWxid { get; set; }

        /// <summary>
        /// 推荐人id
        /// </summary>
        public string ReferorGuid{ get; set; }
         
        public string WeixinID { get; set; }
         

    
        public int? AuditNum { get; set; }
        
        public int? KidNum { get; set; }
        
        
        public decimal? TotalPrice { get; set; }

        public decimal? OrderTotalPrice { get; set; }
        /// <summary>
        /// 实付
        /// </summary>
        public decimal? PayedFee { get; set; }
      
        public DateTime? OrderTime { get; set; }
      
        public DateTime? PayedTime { get; set; }
      
        public decimal? AuditPrice { get; set; }
        
        public decimal? KidPrice { get; set; }
        
        public string Memo { get; set; }
       
        public string CreateBy { get; set; }
       
        public DateTime? CreateDate { get; set; }
       
        public string qdSource { get; set; }
       
        /// <summary>
        ///	选择个还是套餐
        /// </summary> 
        public string LineType { get; set; }
        
        public bool? IsEnable { get; set; }
    
        public decimal? TaocanPrice { get; set; }
      
        public int? TaocanNum { get; set; }
        
 

        public string Transaction_id { get; set; }
      
        public string GoType { get; set; }
       
        public string GoType_Zijia_Adress { get; set; }
       
        public decimal? GoType_bus_Price { get; set; }
      
        public bool? IsSales { get; set; }

        /// <summary>
        ///	特卖ID
        /// </summary> 
        public int? SalesID { get; set; }
       
        public string UJID { get; set; }
      
        public string JID { get; set; }
       
        public int? JiheID { get; set; }
       
        public string JiheAdress { get; set; }
        
        public string OrderState { get; set; }
      
        public bool? IsRefund { get; set; }
       
        public decimal? RefundValue { get; set; }
       
        public DateTime? RefundDate { get; set; }
       
        public decimal? RefundAfterFee { get; set; }
        public string RefundType { get; set; }
      
        public string OrderType { get; set; } 
        public DateTime? BeginTime { get; set; }
      
        public DateTime? EndTime { get; set; }
      
        public int? DayNum { get; set; }
       
        public bool? IsDayCal { get; set; }
      
        public string BizType { get; set; }
     
        public bool? IsOnlyOrder { get; set; }
      
 
        public DateTime? LastCheckOutTime { get; set; }
      
        public string CstIDNumber { get; set; }
 
    
        public string SalersCode { get; set; }
        
      
        public decimal? SharePricePerUnit { get; set; }
      
 
        public int? PointsReturned { get; set; }
       
        public int? PointsNeeded { get; set; }
       
        public decimal? PointToMoney { get; set; }
    
        public int? GrowingValues { get; set; }
       
        public bool? IsUsedPoint { get; set; }
    }
}
