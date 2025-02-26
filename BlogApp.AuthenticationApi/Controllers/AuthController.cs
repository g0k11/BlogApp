using Ardalis.Result;
using BlogApp.AuthenticationApi.Services.Interfaces;
using BlogApp.AuthenticationApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace BlogApp.AuthenticationApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "Username and password are required." });
            }

            var result = await _authService.RegisterAsync(request.Username, request.Password);

            if (!result.IsSuccess)
            {
                return result.Status switch
                {
                    ResultStatus.Invalid => BadRequest(result.Errors),
                    ResultStatus.NotFound => NotFound(result.Errors),
                    ResultStatus.Conflict => Conflict(result.Errors),
                    _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors)
                };
            }

            var userDto = UserResponseDTO.FromEntity(result.Value);
            return Ok(new { Message = "User registered successfully", User = userDto });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "Username and password are required." });
            }

            var result = await _authService.LoginAsync(request.Username, request.Password);

            if (!result.IsSuccess)
            {
                return result.Status switch
                {
                    ResultStatus.Invalid => BadRequest(result.Errors),
                    ResultStatus.Unauthorized => Unauthorized(result.Errors),
                    ResultStatus.NotFound => NotFound(result.Errors),
                    _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Errors)
                };
            }

            return Ok(new { Message = "Login successful", Token = result.Value });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _authService.ChangePasswordAsync(request);
            if (!result.IsSuccess)
            {
                return result.Status switch
                {
                    ResultStatus.NotFound => NotFound(result.Errors),
                    ResultStatus.Unauthorized => Unauthorized(result.Errors),
                    _ => BadRequest(result.Errors)
                };
            }

            return Ok(new { Message = "Password changed successfully." });
        }
    }
}