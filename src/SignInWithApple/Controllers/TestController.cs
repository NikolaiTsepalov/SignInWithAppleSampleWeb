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
    [HttpGet("GetInt")]
    public string GetInt()
    {
        return $"{++_call}";
    }

    private static int _call;
}
