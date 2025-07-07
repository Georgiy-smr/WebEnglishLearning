using DataBaseOperationHelper.BaseCommandsOperations.Сreating;
using Repository.DTO;

namespace Repository.Commands.Create.Users;

public sealed record CreateUserRequest(UserDto NewUserDto) : Create<UserDto>(NewUserDto)
{
    public override string ToString()
    {
        string name = $"{nameof(CreateUserRequest)} {NewUserDto.Name}";
        return name;
    }
}