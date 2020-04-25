/*
 * ************************************
 * file:	    DataPage.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/24 19:32:56
 * description:	
 * ************************************
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm
{
    public  class DataPage<T>
    {
        public DataPage()
        {

        }

        public long CurrentPage { get; set; }
        public long TotalPages { get; set; }
        public long TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public List<T> Items { get; set; } = new List<T>(); 
    }
}
