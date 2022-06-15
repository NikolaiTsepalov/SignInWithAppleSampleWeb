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

    [HttpGet("GetStr")]
    public string GetStr()
    {
        _str += " *";
        return _str;
    }
    private static string _str ="*";
    private static int _call;
}
