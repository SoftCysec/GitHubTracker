namespace GitHubRepoTrackerFE_Blazor.Interfaces
{
    public interface IApiAuthService
    {
        
            Task<string> GetAccessTokenAsync();
        
    }
}
