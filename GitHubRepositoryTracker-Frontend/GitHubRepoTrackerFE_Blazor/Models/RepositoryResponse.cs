namespace GitHubRepoTrackerFE_Blazor.Models
{
    public class RepositoryResponse
    {
        public Repository[] data { get; set; }

        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }
}
