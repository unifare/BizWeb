using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.DistributeSystemPlugin.Model
{
    public class AgentUser
    {
        public long Id { get; set; }
        public string Guid { get; set; }

        public string  UserId { get; set; }
        public string SalesCodeAli { get; set; }
        public string Name { get; set; }
 
        public int? Age { get; set; }
 
        public bool? Sex { get; set; }
 
        public string PhoneNumer { get; set; }
 
        public string SalesCode { get; set; }
 
        public string WechatOpenId { get; set; }
   
        public string ProxyOpenId { get; set; }
  
        public string Email { get; set; }
     
        public string Address { get; set; }
     
        public string BankCardNo { get; set; }
     
        public string BankOwnerName { get; set; }
 
        public string BankName { get; set; }
  
        public string Memo { get; set; }
 
        public string ProxyType { get; set; }
   
        public string ProxyStatus { get; set; }
   
        public int ProxyRealNameStatus { get; set; }
     
        public int? ProxyLevel { get; set; }
         
        public DateTime? RegTime { get; set; }
    
        public string WeChatQrcode { get; set; }
   
        public int? NewSubcribeTimes { get; set; }
 
        public int? NoSubcribeTimes { get; set; }
   
        public int? SubcribePeopleNum { get; set; }
    
        public DateTime? LastScanTime { get; set; }
     
        public bool IsEnable { get; set; }
      
 
        public string FeeAccountNO { get; set; }
     
        public string IdentityNo { get; set; }
 
        public string Profession { get; set; }
        
        public string City { get; set; }
       
        public string LiveLocation { get; set; }

        public DateTime? AddTime { get; set; }
    }
}
