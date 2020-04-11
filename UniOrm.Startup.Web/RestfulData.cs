using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Startup.Web
{
    /// <summary>
    ///
    /// </summary>
    public class RestfulData
    {
        /// <summary>
        /// <![CDATA[错误码]]>
        /// </summary>
        public int code { get; set; }

        /// <summary>
        ///<![CDATA[消息]]>
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// <![CDATA[相关的链接帮助地址]]>
        /// </summary>
        public string url { get; set; }

    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestfulData<T> : RestfulData
    {
        /// <summary>
        /// <![CDATA[数据]]>
        /// </summary>
        public virtual T data { get; set; }
    }

    /// <summary>
    /// <![CDATA[返回数组]]>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestfulArray<T> : RestfulData<IEnumerable<T>>
    {

    }

    public class AccessTokenObj
    {
         public string AccessToken { get; set; }
        public DateTime Expires { get; set; }
        public string Isuser { get; set; }
        public string Audience { get; set; }

    }
}
