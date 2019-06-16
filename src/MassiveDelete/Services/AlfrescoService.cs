using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MassiveDelele.Models;
using MassiveDelete.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Core;

namespace MassiveDelete.Services
{
    public class AlfrescoService
    {
        readonly AppSetting setting;
        readonly IHttpClientFactory httpClientFactory;
        readonly Logger logger;
        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        public AlfrescoService(AppSetting setting, IHttpClientFactory httpClientFactory, Logger logger)
        {
            this.setting = setting;
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        private string GetAuthenticationToken()
        {
            var user = $"{setting.Alfresco.UserName}:{setting.Alfresco.Password}";
            var bytesUser = Encoding.UTF8.GetBytes(user);
            var token = Convert.ToBase64String(bytesUser);
            return token;
        }

        /// <summary>
        /// Search Alfresco Node by Query.
        /// </summary>
        public async Task<AlfrescoSearchOutput> SearchNode(string query)
        {

            var client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GetAuthenticationToken()); // Attach Token

            var searchQuery = new AlfrescoSearchInput
            {
                Query = new AlfrescoSearchQueryInput
                {
                    Query = query
                },
                Paging = new AlfrescoSearchPagingInput
                {
                    MaxItems = setting.MaxItemSearch.ToString(),
                    SkipCount = "0"
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(searchQuery, jsonSettings), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{setting.Alfresco.BaseUrl}/api/-default-/public/search/versions/1/search", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var output = JsonConvert.DeserializeObject<AlfrescoSearchOutput>(responseContent);
                return output;
            }
            else
            {
                logger.Error("Search node from Alfresco fail!");
                logger.Error(responseContent);
            }
            return null;
        }

        /// <summary>
        /// Delete node from Alfresco.
        /// </summary>
        /// <return>
        /// true if success, otherwise false.
        /// </return>
        public async Task<bool> DeleteNode(string uuid)
        {
            var client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GetAuthenticationToken()); // Attach Token
            var response = await client.DeleteAsync($"{setting.Alfresco.BaseUrl}/api/-default-/public/alfresco/versions/1/nodes/{uuid}?permanent=true");

            return response.IsSuccessStatusCode; // Alfresco will return NoContent(204) if suucess.
        }
    }
}