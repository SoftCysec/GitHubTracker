using System.Threading.Tasks;

namespace GitRepositoryTracker.Interfaces
{
    public interface IOpenAIAssistantService
    {
        Task<string> GetOpenAIResponse(string prompt);
    }
}