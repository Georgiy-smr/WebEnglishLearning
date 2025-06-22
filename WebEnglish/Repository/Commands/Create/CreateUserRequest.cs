using Repository.DTO;

namespace Repository.Commands.Create;

public sealed record CreateUserRequest(UserDto NewUserDto) : BaseCreateCommand<UserDto>(NewUserDto)
{
    public override string ToString()
    {
        string name = $"{nameof(CreateUserRequest)} {NewUserDto.Name}";
        return name;
    }
}