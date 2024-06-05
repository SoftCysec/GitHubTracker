using System.Collections.Generic;

namespace GitRepositoryTracker.Models
{
    public class OpenAIResponse
    {
        public List<Choice> Choices { get; set; }
    }

    public class Choice
    {
        public string Text { get; set; }
    }
}