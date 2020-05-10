using System;
using System.ComponentModel.DataAnnotations;

namespace ADBee.Models
{
    public class Advertisement
    {
        [Key] //将来在数据库对应的表中 就是主键
        public long Id { get; set; }
        [MaxLength(30), Required]
        public string Ad_Uuid { get; set; }

        public string PublishUser { get; set; }

        public string AdType { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string AdUrl { get; set; }

        public DateTime AddTime { get; set; }
    }
}
