namespace GitHubRepoTrackerFE_Blazor.Interfaces
{
    public interface IOpenAIAssistantService
    {
        Task<string> GetResponseAsync(string input);
    }
}