namespace MassiveDelele.Models
{
    public class AppSetting
    {
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