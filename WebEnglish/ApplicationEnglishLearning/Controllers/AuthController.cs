using ApplicationEnglishLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Commands.Create;
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
    public async Task<IActionResult> Register([FromBody] UserToRegister userRegister)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        //TODO
        //Создать пользователя с хешированым паролем
        //Вернуть JWT токен.

        CreateUserRequest createCommand = new CreateUserRequest(new UserDto(userRegister.UserName, userRegister.PassWord));

        IStatusGeneric resultAddedNewUser = await _repository.DataBaseOperationAsync(createCommand);

        return Ok(resultAddedNewUser.IsValid);
    }
}