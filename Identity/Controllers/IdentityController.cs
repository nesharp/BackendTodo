using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

public class IdentityController:ControllerBase
{
    private const string TokenSecret = "secret123";
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(24);

    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] TokenGenerationRequest request)
    {
        
    }
}