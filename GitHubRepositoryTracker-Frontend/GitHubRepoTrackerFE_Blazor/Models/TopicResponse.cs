namespace GitHubRepoTrackerFE_Blazor.Models
{
    public class TopicResponse
    {
        public Topic[] data { get; set; }

        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }
}
