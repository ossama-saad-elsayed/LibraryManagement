using LibraryManagement.DTOS;
using LibraryManagement.Services;
using LibraryManagement.Services.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        

        public readonly IAuthService _authService;

        public   AuthController (IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
            {
                var token = await _authService.LoginAsync(request);

                if (token == null)
                {
                    return Unauthorized(new { message = "Invalid email or ID." });
                }

                return Ok(new
                {
                    token = token
                });
            }
        }

    }

