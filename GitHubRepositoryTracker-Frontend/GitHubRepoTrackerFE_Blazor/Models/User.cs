using System.ComponentModel.DataAnnotations;

namespace GitHubRepoTrackerFE_Blazor.Models
{
    public class User
    {
        public string userName;

        [DataType(DataType.Password)]
        public string password;
    }
}
