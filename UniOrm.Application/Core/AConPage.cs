using System.Collections.Generic;

namespace UniOrm.Core
{
    public class AConPage:BaseElement
    {
        public Layout Layout { get; set; }
        public List<PageCheckPoint> PageCheckPoints { get; set; }
    }
}