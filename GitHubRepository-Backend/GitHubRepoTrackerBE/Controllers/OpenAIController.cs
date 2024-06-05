using GitRepositoryTracker.Interfaces;
using GitRepositoryTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GitRepositoryTracker.Services;

namespace GitRepositoryTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIController : ControllerBase
    {
        private readonly IOpenAIAssistantService _openAIAssistantService;
        private readonly IGitHubAPIService _gitHubAPIService;
        private readonly ILogger<OpenAIController> _logger;

        public OpenAIController(IOpenAIAssistantService openAIAssistantService, IGitHubAPIService gitHubAPIService, ILogger<OpenAIController> logger)
        {
            _openAIAssistantService = openAIAssistantService;
            _gitHubAPIService = gitHubAPIService;
            _logger = logger;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion([FromBody] OpenAIRequest request)
        {
            _logger.LogInformation("Received prompt: {Prompt}", request.Prompt);

            // Fetch data from GitHub based on the prompt (this is a simplistic example, modify as needed)
            var gitHubData = await _gitHubAPIService.GetStatisticsAsync(request.Prompt);

            // Combine data and prompt
            var combinedPrompt = $"{request.Prompt}\n Here are some statistics: {gitHubData}";

            var response = await _openAIAssistantService.GetOpenAIResponse(combinedPrompt);
            _logger.LogInformation("Generated response: {Response}", response);

            return Ok(new { Response = response });
        }
    }
}