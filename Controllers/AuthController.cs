using Microsoft.AspNetCore.Mvc;
using ShopList.Models;

namespace ShopList.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(ApplicationDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest model)
    {
        var user = _context.Users.SingleOrDefault(u => u.UserName == model.UserName && u.PasswordHash == model.PasswordHash);
        if (user is null)
        {
            return Unauthorized("Invalid username or password");
        }

        var token = _jwtService.GenerateToken(user);
        return Ok(new { Token = token });
    }
}
