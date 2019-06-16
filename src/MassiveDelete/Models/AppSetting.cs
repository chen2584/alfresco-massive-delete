namespace MassiveDelele.Models
{
    public class AppSetting
    {
        public AlfrescoSetting Alfresco { get; set; }
        public string SearchQuery { get; set; }
        public int MaxItemSearch { get; set; }
        public int MaxHttpClient { get; set; }
        public int SleepTime { get; set; }
    }

    public class AlfrescoSetting
    {
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}