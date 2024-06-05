using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using GitHubRepoTrackerFE_Blazor.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GitHubRepoTrackerFE_Blazor.Services
{
    public class OpenAIAssistantService : IOpenAIAssistantService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenAIAssistantService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetResponseAsync(string input)
        {
            var request = new { model = "gpt-4", prompt = input, max_tokens = 100 };
            var apiKey = _configuration["OpenAI:ApiKey"];
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/completions", request);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                return responseBody?.Choices?.FirstOrDefault()?.Text?.Trim() ?? "No response";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase}). Error: {error}");
            }
        }

        private class OpenAIResponse
        {
            public List<Choice> Choices { get; set; }
        }

        private class Choice
        {
            public string Text { get; set; }
        }
    }
}