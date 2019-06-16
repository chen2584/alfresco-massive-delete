using System;

namespace MassiveDelete.Models
{
    public class AlfrescoFileInfo
    {
        public AlfrescoEntryFileInfo Entry { get; set; }
    }

    public class AlfrescoEntryFileInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}