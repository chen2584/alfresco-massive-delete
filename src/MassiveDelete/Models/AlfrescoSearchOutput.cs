using System.Collections.Generic;

namespace MassiveDelete.Models
{
    public class AlfrescoSearchOutput
    {
        public AlfrescoSearchListOutput List { get; set; }
    }

    public class AlfrescoSearchListOutput
    {
        public AlfrescoSearchPaginationOutput Pagination { get; set; }
        public IEnumerable<AlfrescoFileInfo> Entries { get; set; }
    }

    public class AlfrescoSearchPaginationOutput
    {
        public int TotalItems { get; set; }
    }
}