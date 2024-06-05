using GitHubRepoTrackerFE_Blazor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubRepoTrackerFE_Blazor.Interfaces
{
    public interface ILanguageService
    {
        Task<List<Language>> GetLanguagesAsync();
    }
}