using DataBaseOperationHelper.BaseCommandsOperations.Сreating;
using Repository.DTO;

namespace Repository.Commands.Create;

public sealed record RequestCreateUserRequest(UserDto NewUserDto) : Create<UserDto>(NewUserDto)
{
    public override string ToString()
    {
        string name = $"{nameof(RequestCreateUserRequest)} {NewUserDto.Name}";
        return name;
    }
}