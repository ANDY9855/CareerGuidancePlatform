using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace CareerGuidancePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ChatbotController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("Message cannot be empty.");
            }

            var apiKey = _configuration["Gemini:ApiKey"];
            var apiUrl = _configuration["Gemini:ApiUrl"];

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiUrl))
            {
                return StatusCode(500, "Chatbot is not configured properly.");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                
                // Gemini expects the API key as a query parameter
                var requestUrl = $"{apiUrl}?key={apiKey}";

                var payload = new
                {
                    system_instruction = new
                    {
                        parts = new[]
                        {
                            new { text = "You are the official AI Career Assistant for 'CareerPath'. CareerPath is a comprehensive Career Guidance Platform. The website has a built-in Job Board where users can directly find and apply for jobs. It also features a Resume Builder, Career Assessments, and a Mentorship matching system. Do not say you are just an AI; act as the integrated guide for this specific website. Keep your answers concise, helpful, and professional." }
                        }
                    },
                    contents = new[]
                    {
                        new { parts = new[] { new { text = request.Message } } }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return StatusCode(500, new { error = $"API Error ({response.StatusCode})", details = errorResponse });
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                
                using var document = JsonDocument.Parse(jsonResponse);
                var root = document.RootElement;
                
                string reply = "I'm sorry, I couldn't understand the response from the AI.";

                // Parse Gemini response format
                if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    var firstCandidate = candidates[0];
                    if (firstCandidate.TryGetProperty("content", out var contentProp) && 
                        contentProp.TryGetProperty("parts", out var parts) && 
                        parts.GetArrayLength() > 0)
                    {
                        var firstPart = parts[0];
                        if (firstPart.TryGetProperty("text", out var textProp))
                        {
                            reply = textProp.GetString() ?? reply;
                        }
                    }
                }
                else
                {
                    // Fallback debug
                    reply = jsonResponse;
                }

                return Ok(new { reply = reply });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while communicating with the AI.", details = ex.Message });
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }
}
