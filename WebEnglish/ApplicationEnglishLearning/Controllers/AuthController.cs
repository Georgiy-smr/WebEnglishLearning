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
    private readonly IRepository _repository;
    private readonly IGenerateToken _generateToken;
    private readonly IAuthService _hashService;


    public AuthController(
        IRepository repository,
        IGenerateToken generateToken,
        IAuthService hashService)
    {
        _repository = repository;
        _generateToken = generateToken;
        _hashService = hashService;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserToAuth userAuth)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        CreateUserRequest createCommand = new CreateUserRequest(
            new UserDto(userAuth.UserName, 
                _hashService.Hash(userAuth.PassWord).GetHashed()));

        IStatusGeneric resultAddedNewUser = await _repository.DataBaseOperationAsync(createCommand);

        return Ok(resultAddedNewUser.IsValid);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserToAuth userAuth)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        GetUsersRequest usersRequest = new GetUsersRequest()
        {
            Filters = new List<Expression<Func<User, bool>>>()
            {
                x => x.UserName == userAuth.UserName,
            },
            Includes = new List<Expression<Func<User, object>>>()
            {
            },
            Size = 1,
            ZeroStart = 0
        };
        IStatusGeneric<IEnumerable<UserDto>> getUserCommand = await _repository.GetItemsAsync(usersRequest);
        if (!getUserCommand.IsValid)
        {
            return BadRequest(ModelState);
        }
        UserDto userInDataBase = getUserCommand.Result.Single();

        if (!_hashService.CreateValidating(userInDataBase.HashPass).Validate(userAuth.PassWord))
            return BadRequest(401);

        var token = _generateToken.Generate(userInDataBase);
        HttpContext.Response.Cookies.Append("jwt", token);

        return Ok(token);
    }

}