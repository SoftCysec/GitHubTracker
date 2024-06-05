using GitHubRepoTrackerFE_Blazor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitHubRepoTrackerFE_Blazor.Pages;

namespace GitHubRepoTrackerFE_Blazor.Interfaces
{
    public interface IRepoService
    {
        Task<List<Repository>> GetReposAsync();
        IEnumerable<Repository> ReposPerTopic(string topic);
        IEnumerable<Repository> ReposPerLanguage(string language);
    }
}