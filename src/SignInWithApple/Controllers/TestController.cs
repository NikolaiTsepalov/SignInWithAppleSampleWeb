using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace MartinCostello.SignInWithApple.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TestController:  ControllerBase
{
    [AllowAnonymous]
    [HttpGet("Hello")]
    public string Hello()
    {
        return "Hello Stranger!";
    }

    [HttpGet("Hi")]
    public string Hi()
    {
        return "Hi man!";
    }
}
