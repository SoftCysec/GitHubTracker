using GitRepositoryTracker.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Octokit;
using Polly;
using System.Text;
using System.Threading.Tasks;

namespace GitRepositoryTracker.Services
{
    public class GitHubAPIService : IGitHubAPIService
    {
        private readonly IGitHubClient _gitHubClient;
        private IMemoryCache _memoryCache;

        public GitHubAPIService(IGitHubClient gitHubClient, IMemoryCache memoryCache)
        {
            _gitHubClient = gitHubClient;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Repository>> GetAllRepositoriesBySize(int size, int page, int perPage)
        {
            var repositories = new List<Repository>();
            var searchRequest = new SearchRepositoriesRequest
            {
                Size = Octokit.Range.GreaterThanOrEquals(size),
                Page = page,
                PerPage = perPage,
                SortField = RepoSearchSort.Stars
            };

            var searchResult = await SearchRepositoriesWithRetry(searchRequest);
            repositories.AddRange(searchResult.Items);

            while (searchResult?.IncompleteResults == false)
            {
                searchRequest.Page++;
                searchResult = await SearchRepositoriesWithRetry(searchRequest);
                repositories.AddRange(searchResult.Items);
            }

            return repositories;
        }

        public async Task<string> GetStatisticsAsync(string prompt)
        {
            // This method would process the prompt and retrieve statistics from GitHub
            // Based on your requirements. For now, let's build a sample implementation.

            if (prompt.Contains("Rust", System.StringComparison.OrdinalIgnoreCase))
            {
                var rustRepos = await GetAllRepositoriesBySize(0, 1, 10); // Fetch a subset for demonstration
                StringBuilder stats = new StringBuilder();
                stats.AppendLine($"Total Rust repositories found: {rustRepos.Count()}");
                
                // Example: Collect more detailed statistics as needed
                foreach (var repo in rustRepos)
                {
                    stats.AppendLine($"{repo.FullName} - {repo.StargazersCount} stars");
                }

                return stats.ToString();
            }

            // Placeholders for other prompts
            return "Statistic fetching not implemented for this query.";
        }

        private async Task<SearchRepositoryResult> SearchRepositoriesWithRetry(SearchRepositoriesRequest searchRequest)
        {
            var retryPolicy = Policy
                .Handle<SecondaryRateLimitExceededException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var cacheKey = $"RepoSearch:{searchRequest.Size}:{searchRequest.Page}:{searchRequest.PerPage}:{searchRequest.SortField}";

            SearchRepositoryResult searchResult;
            if (!_memoryCache.TryGetValue(cacheKey, out searchResult))
            {
                searchResult = await retryPolicy.ExecuteAsync(() => _gitHubClient.Search.SearchRepo(searchRequest));
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _memoryCache.Set(cacheKey, searchResult, cacheEntryOptions);
            }

            return searchResult;
        }
    }
}