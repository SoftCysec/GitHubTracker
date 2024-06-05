using System.ComponentModel.DataAnnotations;

namespace GitHubRepoTrackerFE_Blazor.Models
{
    public class Repository
    {
        public string RepositoryName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public Language Language { get; set; }
        public Topic[] RepositoryTopics { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }
        public int ForksCount { get; set; }
        public int StargazersCount { get; set; } 
    }
}