using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoBackend.Interfaces;
using TodoBackend.Models;

namespace TodoBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController:ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly DatabaseContext _context;

    public AuthController(IConfiguration configuration, DatabaseContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("register")]
    public ActionResult Register(UserDto request)
    {
        //finding user with the same email
        var existingUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        //returning error if user with this email already exists
        if(existingUser != null) return BadRequest("User with this email already exists");
        //creating password hash
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        //creating new user
        User newUser = new User
        {   
            Email = request.Email,
            Name = request.Name,
            PasswordHash = passwordHash,
            Role = UserRole.User
        };
        //creating new user in database and uploading it to database
        _context.Users.Add(newUser);
        _context.SaveChanges();
        //creating token for auth
        string token = CreateToken(newUser); 
        //returning ok result with user and token
        return Ok(
        new {
            user = newUser,
            token
        });
    }

    [HttpPost("login")]
    public ActionResult<User> Login([FromBody]UserLoginDto reqest)
    {
        //getting user from database by email
        var user = _context.Users.FirstOrDefault(u => u.Email == reqest.Email);
        //returning error if user with this email doesn't exist
        if(user?.Email != reqest.Email)
        {
            return BadRequest("Login or password are incorrect");
        }
        //returning error if password is incorrect
        if (!BCrypt.Net.BCrypt.Verify(reqest.Password, user.PasswordHash))
        {
            return BadRequest("Login or password are incorrect");
        }
        //creating token for auth
        string token = CreateToken(user);
        //returning ok result with user and token
        return Ok(new {user, token});
    }
    [HttpGet, Authorize]
    public ActionResult<string> GetName()
    {
        Console.WriteLine($"Full name: {User.Identity?.Name}");
        Console.WriteLine($"Email: {User.FindFirst(ClaimTypes.Email)?.Value}");
        List<User> users = _context.Users.ToList();
        return Ok(users);
    }
    
    [HttpGet("profile"), Authorize]
    public ActionResult<User> Profile()
    {
        string email = User.FindFirst(ClaimTypes.Email)?.Value;
        if(email == null) return BadRequest("Something went wrong");
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if(user == null) return BadRequest("Something went wrong");
        
        return Ok(user);
    }
    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}

