using Newtonsoft.Json;

namespace GitHubRepoTrackerFE_Blazor.Models
{
    public class AccessToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expiration")]
        public DateTime ExpirationTime { get; set; }
    }
}
