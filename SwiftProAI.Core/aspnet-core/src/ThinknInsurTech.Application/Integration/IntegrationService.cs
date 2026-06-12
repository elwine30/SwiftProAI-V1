using Abp.UI;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThinknInsurTech.Integration.Dto;

namespace ThinknInsurTech.Integration
{
    public class IntegrationService : IIntegrationService
    {
        #region Declarations
        private readonly IHttpClientFactory _clientFactory;
        #endregion

        #region Constructor
        public IntegrationService(
            IHttpClientFactory clientFactory
        )
        {
            _clientFactory = clientFactory;
        }
        #endregion

        /// <summary>
        /// POST request to ChatGPT
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Response from ChatGPT based on input content</returns>
        public async Task<string> ChatGPTCompletions(ChatGPTInputDto input)
        {
            var json = JsonConvert.SerializeObject(input);

            var client = CreateChatGPTHttpClient();

            var response = await client.PostAsync("/v1/chat/completions", new StringContent(json, Encoding.UTF8, "application/json"));

            var responseStream = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var normalizedResponse = responseStream.Replace("\n", "").Replace("\r", "");

                // Check for isAccurateDocument
                if (normalizedResponse.Contains("\\\"isAccurateDocument\\\": false"))
                {
                    throw new UserFriendlyException("This seemed to be a wrong document. Please upload the correct document type.");
                }
                return responseStream;
            }
            else
            {
                var deserialize = JsonConvert.DeserializeObject<ChatGPTResponseDto>(responseStream);

                throw new UserFriendlyException($"{deserialize.error.message}");
            }
        }

        /// <summary>
        /// Create HttpClient for ChatGpt requests
        /// </summary>
        /// <returns>Created HttpClient</returns>
        private HttpClient CreateChatGPTHttpClient()
        {
            var httpClient = _clientFactory.CreateClient();

            httpClient.BaseAddress = new Uri("https://api.openai.com");

            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer sk-svcacct-z1yrMXMIMgUDsutRNsCO1Krilv-Nj_sCkR96w1K50R0vcSwE_m4eGnIlwVivu_PFDSRxi-PEJkwQ0bkXtSyHT3BlbkFJv_lGs8futZyqZFjFCGAQX-v3b5MaOlG2mRgMWUvSTiVZvEUDKDE-p31HyCQh2LfkS6JE00IFVhsP0CLFsh4A");

            return httpClient;
        }
    }
}
