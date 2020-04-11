namespace UniOrm.Core 
{
    public class Section: BaseElement
    {
        public string DisplayText { get; set; }
        public string Href { get; set; }
        public AConAuthorize Authorize { get; set; }
        
    }
}