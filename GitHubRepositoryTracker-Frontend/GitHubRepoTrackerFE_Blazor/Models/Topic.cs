namespace GitHubRepoTrackerFE_Blazor.Models
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public int Popularity { get; set; } // Replace 'SomeMetric' in Analytics.razor
    }
}