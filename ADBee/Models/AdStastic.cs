using System;
using System.ComponentModel.DataAnnotations;

namespace ADBee.Models
{
    public class AdStastic
    {
        [Key] //将来在数据库对应的表中 就是主键
        public long Id { get; set; }
        [MaxLength(30), Required]
        public string Ad_Uuid { get; set; }

        public string UserName { get; set; } 

        public DateTime TimeStartedDate { get; set; }

        public DateTime Click_Time { get; set; }

        public string Ip4Client { get; set; }
        public int Ip4_1 { get; set; }
        public int Ip4_2 { get; set; }
        public int Ip4_3 { get; set; }
        public int Ip4_4 { get; set; }

        public string Ip6Client { get; set; } 

        public string RefUrl { get; set; }

        public DateTime AddTime { get; set; }
    }
}
