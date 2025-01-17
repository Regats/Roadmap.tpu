using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadmapDesigner.Server.Interfaces;

namespace RoadmapDesigner.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<DirectionTrainingController> _logger;

        public AuthenticationController (ILogger<DirectionTrainingController> logger)
        {
            _logger = logger;
        }

        [HttpPost("token")]
        public async Task<IActionResult> ExchangeCode([FromBody] ExchangeCodeRequest request)
        {
            using var client = new HttpClient();

            var response = await client.PostAsync("https://oauth.tpu.ru/access_token", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", "Ваш client_id" },
                { "client_secret", "Ваш client_secret" },
                { "code", request.Code },
                { "grant_type", "authorization_code" },
                { "redirect_uri", "https://localhost:5173/callback" }
            }));

            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(content);
            }

            return Ok(content);
        }

        public class ExchangeCodeRequest
        {
            public string Code { get; set; }
        }
    }
}
