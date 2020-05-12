/*
 * ************************************
 * file:	    AConMvcClass.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/5/10 9:18:37
 * description:	
 * ************************************
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class AConMvcClass
    {
        public string VersionNum { get; set; }

        public long Id { get; set; }
        public string Guid { get; set; }

        public string Name { get; set; }

        public bool? IsController { get; set; } = true;

        public bool? IsSelfDefine { get; set; } = false;

        public string InhiredClass { get; set; }

        public string UsingNameSpance {get;set;}

        public string ExReferenceName { get; set; }

        public string ActionCode { get; set; }

        public string ClassName { get; set; }

        public string UrlRule { get; set; }

        public bool? IsEanable { get; set; } = false;

        public string ClassAttrs { get; set; }

        public DateTime? Addtime { get; set; }
    }
}
