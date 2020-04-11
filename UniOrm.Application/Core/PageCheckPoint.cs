using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniOrm.Core
{
    public class PageCheckPoint:BaseElement
    {
        public AConPage AConPage { get; set; }
        public bool IsEnable { get; set; }
    }
}
