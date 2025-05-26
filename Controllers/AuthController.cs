using Microsoft.AspNetCore.Mvc;
using MyApi.Data;


[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserModel user)
    {
        if (_db.Users.Any(u => u.Name == user.Name))
            return BadRequest("Username already exists.");

        user.PasswordHash = AuthHelper.HashPassword(user.PasswordHash);
        _db.Users.Add(user);
        _db.SaveChanges();

        return Ok("User created.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserModel credentials)
    {
        var hashed = AuthHelper.HashPassword(credentials.PasswordHash);
        var user = _db.Users.FirstOrDefault(u => u.Name == credentials.Name && u.PasswordHash == hashed);
        if (user == null) return Unauthorized();

        var token = AuthHelper.GenerateJwtToken(user.Id, _config["JwtKey"]!);
        return Ok(new { token });
    }
}
