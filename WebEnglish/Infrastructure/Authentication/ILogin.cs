using Entities;
using Microsoft.AspNetCore.Http;
using Repository;
using Repository.Commands.Read;
using Repository.DTO;
using StatusGeneric;
using System.Linq;
using System.Linq.Expressions;
using Repository.Commands.Create;

namespace Infrastructure.Authentication;

public interface ILogin
{
    Task<IStatusGeneric<string>> CreateToken(
        string userName,
        string password,
        CancellationToken cancellationToken = default);
    Task<IStatusGeneric> Register(
        string userName,
        string password,
        CancellationToken cancellationToken = default);
}

public class UserService : ILogin
{
    private readonly IAuthService _hashService;
    private readonly IRepository _repository;
    private readonly IGenerateToken _generateToken;
    public UserService(IAuthService authService, IRepository repository, IGenerateToken generateToken)
    {
        _hashService = authService;
        _repository = repository;
        _generateToken = generateToken;
    }

    public async Task<IStatusGeneric<string>> CreateToken(string userName, string password, CancellationToken cancellationToken = default)
    {
        var status = new StatusGenericHandler<string>()
        {
            Header = $"Аутентификация пользователя {userName}"
        };

        GetUsersRequest usersRequest = new GetUsersRequest()
        {
            Filters = new List<Expression<Func<User, bool>>>()
            {
                x => x.UserName == userName,
            },
            Includes = new List<Expression<Func<User, object>>>()
            {
            },
            Size = 1,
            ZeroStart = 0
        };
        IStatusGeneric<IEnumerable<UserDto>> getUserCommand = await _repository.GetItemsAsync(usersRequest, cancellationToken);
        if (getUserCommand.HasErrors)
            return status.AddError(string.Join(";", getUserCommand.Errors));

        UserDto userInDataBase = getUserCommand.Result.Single();

        if (!_hashService.CreateValidating(userInDataBase.HashPass).Validate(password))
            return status.AddError("Неправильный пароль");

        return status.SetResult(_generateToken.Generate(userInDataBase));
    }

    public async Task<IStatusGeneric> Register(
        string userName,
        string password,
        CancellationToken cancellationToken = default)
    {
        CreateUserRequest createCommand = new CreateUserRequest(
            new UserDto(userName,
                _hashService.Hash(password).GetHashed()));

        return await _repository.DataBaseOperationAsync(createCommand, cancellationToken);
    }
}
