using HealthApp.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HealthApp.Api.Controller.AuthController
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _token;

        public AuthController(ITokenService token)
        {
            _token = token;
        }

        [HttpPost]
        public async Task<IActionResult> login(LoginRequest login)
        {
            if (login.Email != "devaraut1997@gmail.com" || login.Password != "admin")
                return Unauthorized();

            var token = _token.GenerateToken(1, login.Email);

            return Ok(token);
        }
    }
}
