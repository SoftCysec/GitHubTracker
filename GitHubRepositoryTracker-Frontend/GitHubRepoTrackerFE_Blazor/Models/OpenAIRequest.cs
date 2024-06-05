namespace GitHubRepoTrackerFE_Blazor.Models
{
    public class OpenAIRequest
    {
        public string Model { get; set; }
        public string Prompt { get; set; }
        public int MaxTokens { get; set; }
        public string AssistantId { get; set; }
    }
}