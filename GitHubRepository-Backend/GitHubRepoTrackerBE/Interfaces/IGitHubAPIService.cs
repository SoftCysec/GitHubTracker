using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitRepositoryTracker.Interfaces
{
    public interface IGitHubAPIService
    {
        Task<IEnumerable<Repository>> GetAllRepositoriesBySize(int size = 2000, int page = 2, int perPage = 10);
        Task<string> GetStatisticsAsync(string prompt); // Added for fetching statistics
    }
}