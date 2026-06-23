using System.Text;
using System.Text.Json;

namespace AgenticCrm.Services;

public class AnthropicService
{
    private readonly HttpClient _http;
    private readonly bool _active;

    public bool IsActive => _active;

    public AnthropicService(HttpClient http, IConfiguration config)
    {
        _http = http;
        var key = config["ANTHROPIC_API_KEY"] ?? config["Anthropic:ApiKey"];
        _active = !string.IsNullOrWhiteSpace(key) && key != "MY_ANTHROPIC_API_KEY";
        if (_active)
        {
            _http.BaseAddress = new Uri("https://api.anthropic.com");
            _http.DefaultRequestHeaders.Add("x-api-key", key);
            _http.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
        }
    }

    public async Task<string> GenerateAsync(string systemPrompt, string userMessage, double temperature = 0.2, int maxTokens = 2048)
    {
        var payload = new
        {
            model = "claude-opus-4-8",
            max_tokens = maxTokens,
            system = systemPrompt,
            messages = new[] { new { role = "user", content = userMessage } },
            temperature
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("/v1/messages", content);
        response.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return doc.RootElement.GetProperty("content")[0].GetProperty("text").GetString() ?? "{}";
    }
}
