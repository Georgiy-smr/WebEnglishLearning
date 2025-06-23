using System.Linq.Expressions;
using ApplicationEnglishLearning.Models;
using Entities;
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

    public AuthController(IRepository repository)
    {
        _repository = repository;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserToAuth userAuth)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //TODO
        //Создать пользователя с хешированым паролем
        //Вернуть JWT токен.


        IPasswordHash hash = new Password(userAuth.PassWord);
        var passwordHashed = hash.GetHashed();
        CreateUserRequest createCommand = new CreateUserRequest(new UserDto(userAuth.UserName, passwordHashed));

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

        IValidatePassword validate = new HashValidateWithException(new HashValidate(userInDataBase.HashPass));

        return Ok(validate.Validate(userAuth.PassWord));
    }

}