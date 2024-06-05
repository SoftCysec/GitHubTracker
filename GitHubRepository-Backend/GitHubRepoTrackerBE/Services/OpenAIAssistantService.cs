using GitRepositoryTracker.Interfaces;
using GitRepositoryTracker.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitRepositoryTracker.Services
{
    public class OpenAIAssistantService : IOpenAIAssistantService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenAIAssistantService> _logger;

        public OpenAIAssistantService(HttpClient httpClient, IConfiguration configuration, ILogger<OpenAIAssistantService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GetOpenAIResponse(string prompt)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogError("OpenAI API Key is missing in configuration.");
                throw new InvalidOperationException("OpenAI API Key is missing in configuration.");
            }

            var requestUri = "https://api.openai.com/v1/chat/completions";
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 100,
                temperature = 0.7
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            _logger.LogInformation("Sending request to OpenAI with prompt: {Prompt}", prompt);

            var response = await _httpClient.PostAsync(requestUri, requestContent);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("OpenAI API call failed: {StatusCode}, {ErrorContent}", response.StatusCode, errorContent);
                throw new HttpRequestException($"OpenAI API call failed: {response.StatusCode}, {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Received response from OpenAI: {ResponseContent}", responseContent);

            var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseContent);
            var result = openAIResponse?.Choices?.FirstOrDefault()?.Text?.Trim() ?? "No response";

            _logger.LogInformation("Processed response: {Response}", result);

            return result;
        }
    }
}