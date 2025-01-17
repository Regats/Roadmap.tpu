using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RoadmapDesigner.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }


        [HttpGet("login")]
        public IActionResult Login()
        {
            // Redirect пользователя на страницу авторизации TPU
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "TPU");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            _logger.LogInformation("Выход пользователя из системы");

            //Выход из системы с очисткой куки
            return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme);
        }
        //  [HttpGet("signin-tpu")] // Важно что бы CallbackPath совпадал с настройкой OAuth провайдера
        // public async Task<IActionResult> SignInTpuCallback()
        // {
        //   var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //   if (!authenticateResult.Succeeded)
        //   {
        //     return BadRequest("Ошибка аутентификации");
        //   }
        //   return LocalRedirect("/");
        // }
    }
}