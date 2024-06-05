namespace GitHubRepoTrackerFE_Blazor.Models
{
    public class PagingInfo
    {
            public string Text { get; set; }
            public int PageIndex { get; set; }
            public bool Enabled { get; set; }
            public bool Active { get; set; }

            public PagingInfo(int pageIndex, bool enabled, string text)
            {
                this.PageIndex = pageIndex;
                this.Enabled = enabled;
                this.Text = text;
            }
        
    }
}
