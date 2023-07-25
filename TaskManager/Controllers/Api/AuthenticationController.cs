using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITaskManagerUserRepository _userRepository;

        public AuthenticationController(ITaskManagerUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserModel u)
        {
            var isAuthenticated = await _userRepository.IsAuthenticated(u.DomainName, u.UserName, u.Password);
            
            if (!isAuthenticated)
            {
                return Ok(new { ok = false });
            }

            return Ok(new { ok = true });
        }


    }
}
