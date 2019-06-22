namespace MassiveDelele.Models
{
    public class AppSetting
    {
        public AlfrescoSetting Alfresco { get; set; }
        public string SearchQuery { get; set; }
        public int WorkerNumber { get; set; }
        public int MaxSearchItem { get; set; }
        public int SearchDelay { get; set; }
    }

    public class AlfrescoSetting
    {
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}