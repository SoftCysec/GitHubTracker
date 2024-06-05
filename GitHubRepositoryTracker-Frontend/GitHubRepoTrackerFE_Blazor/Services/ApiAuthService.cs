using GitHubRepoTrackerFE_Blazor.Interfaces;
using GitHubRepoTrackerFE_Blazor.Models;
using Newtonsoft.Json;
using System.Text;

namespace GitHubRepoTrackerFE_Blazor.Services
{
    public class ApiAuthService : IApiAuthService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiAuthService> _logger;
        public ApiAuthService(HttpClient client, IConfiguration configuration, ILogger<ApiAuthService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            string token = "";

            var user = new User()
            {
                userName = _configuration.GetValue<string>("TokenUserName"),

                password = _configuration.GetValue<string>("TokenPassword")
            };

            var builder = new UriBuilder(_client.BaseAddress + _configuration.GetValue<string>("ApiEndpoints:GetAccessToken"));
            var url = builder.ToString();

            var userJson = JsonConvert.SerializeObject(user);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            try
            {
                var res = await _client.PostAsync(url, data);

                if (res.IsSuccessStatusCode)
                {


                    var result = await res.Content.ReadAsStringAsync();
                    var deserializedRes = JsonConvert.DeserializeObject<AccessToken>(result);
                    token = deserializedRes.Token;
                    _logger.LogInformation("Token successful");


                }
              

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex}");
            }

           
            return token;
        }

    }
}
