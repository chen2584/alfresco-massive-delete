namespace MassiveDelele.Models
{
    public class AlfrescoSearchInput
    {
        public AlfrescoSearchQueryInput Query { get; set; }
        public AlfrescoSearchPagingInput Paging { get; set; }
    }

    public class AlfrescoSearchQueryInput
    {
        public string Query { get; set; }
    }

    public class AlfrescoSearchPagingInput
    {
        public string MaxItems { get; set; }
        public string SkipCount { get; set; }
    }
}