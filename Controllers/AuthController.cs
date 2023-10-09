using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopList.Models;

namespace ShopList.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;

    public AuthController(IUserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        var user = await _userRepository.GetUserByUsernameAsync(model.UserName);
        if (user == null || user.PasswordHash != model.PasswordHash)
        {
            return Unauthorized("Invalid username or password");
        }

        var token = _jwtService.GenerateToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest model)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(model.UserName);
        if (existingUser != null)
        {
            return BadRequest("Username already exists");
        }

        var newUser = new User
        {
            UserName = model.UserName,
            PasswordHash = model.PasswordHash // Note: You should hash the password securely before storing it
        };

        await _userRepository.CreateUserAsync(newUser);

        return Ok("User registered successfully");
    }

    [HttpGet("me")]
    [Authorize] // Requires authentication
    public async Task<IActionResult> GetCurrentUser()
    {
    var username = User.Identity.Name;

    var user = await _userRepository.GetUserByUsernameAsync(username);
    if (user is null)
    {
        return NotFound("User not found");
    }

    // You can customize the response based on your user entity structure
    var response = new
    {
    user.Id,
    user.UserName,
    // Add other properties as needed
    };

    return Ok(response);
    }
}

