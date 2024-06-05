using GitHubRepoTrackerFE_Blazor.Interfaces;
using GitHubRepoTrackerFE_Blazor.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace GitHubRepoTrackerFE_Blazor.Services
{
    public class RepoService : IRepoService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IApiAuthService _apiAuthService;
        private readonly ILogger<RepoService> _logger;

        public RepoService(HttpClient client, IConfiguration configuration, IApiAuthService apiAuthService, ILogger<RepoService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration;
            _apiAuthService = apiAuthService;
            _logger = logger;
        }

        public async Task<List<Repository>> GetReposAsync()
        {
            var token = await _apiAuthService.GetAccessTokenAsync();
            var repos = new List<Repository>();

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var pageNumber = 1;
            var pageSize = 20;

            while (true)
            {
                var builder = new UriBuilder(_client.BaseAddress + _configuration.GetValue<string>("ApiEndpoints:GetAllReposEndpoint"));

                var query = HttpUtility.ParseQueryString(builder.Query);
                query["pageNumber"] = pageNumber.ToString();
                query["pageSize"] = pageSize.ToString();
                builder.Query = query.ToString();
                var uri = builder.ToString();

                try
                {
                    var response = await _client.GetAsync(uri);

                    if (!response.IsSuccessStatusCode)
                    {
                        break;
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    var paginatedRepos = JsonConvert.DeserializeObject<RepositoryResponse>(content);

                    if (!paginatedRepos.data.Any())
                    {
                        break;
                    }

                    repos.AddRange(paginatedRepos.data);
                    _logger.LogInformation("Repos retrieved successfully");
                    pageNumber++;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    break;
                }
            }

            return repos;
        }

        public IEnumerable<Repository> ReposPerLanguage(string language)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Repository> ReposPerTopic(string topic)
        {
            throw new NotImplementedException();
        }
    }
}
