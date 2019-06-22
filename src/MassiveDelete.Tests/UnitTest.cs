using System.Reflection;
using MassiveDelele.Models;
using MassiveDelete.Services;
using Xunit;

namespace MassiveDelete.Tests
{
    public class UnitTest
    {
        readonly AlfrescoService alfrescoService;
        readonly AppSetting setting;
        public UnitTest()
        {
            this.setting = new AppSetting
            {
                Alfresco = new AlfrescoSetting
                {
                    UserName = "admin",
                    Password = "password"
                }
            };
            this.alfrescoService = new AlfrescoService(this.setting, null, null);
        }

        [Fact]
        public void GetAuthenticationTest()
        {
            var method = alfrescoService.GetType().GetMethod("GetAuthenticationToken", BindingFlags.NonPublic | BindingFlags.Instance);
            var token = (string)method.Invoke(alfrescoService, null);
            Assert.Equal("YWRtaW46cGFzc3dvcmQ=", token);
        }
    }
}
