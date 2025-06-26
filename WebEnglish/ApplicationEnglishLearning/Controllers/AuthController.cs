using System.Linq.Expressions;
using ApplicationEnglishLearning.Models;
using Entities;
using Infrastructure.Authentication;
using Infrastructure.PasswordHelps;
using Infrastructure.PasswordHelps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Commands.Create;
using Repository.Commands.Read;
using Repository.DTO;
using StatusGeneric;

namespace ApplicationEnglishLearning.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogin _loginService;

    public AuthController(ILogin loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserToAuth userAuth)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        IStatusGeneric resultAddedNewUser = await _loginService.Register(userAuth.UserName, userAuth.PassWord);
        return Ok(resultAddedNewUser);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserToAuth userAuth)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        IStatusGeneric<string> resultLogin = await _loginService.CreateToken(userAuth.UserName, userAuth.PassWord);

        if(resultLogin.HasErrors)
            return BadRequest(resultLogin);

        HttpContext.Response.Cookies.Append("jwt", resultLogin.Result);
        return Ok(resultLogin.Result);
    }

}