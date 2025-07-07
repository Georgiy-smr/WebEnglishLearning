using DataBaseOperationHelper.BaseCommandsOperations.Reading;
using Entities;
using Repository.DTO;

namespace Repository.Commands.Read.Users;

public record GetUsersRequest() : 
        ReadCommandRequest<User,UserDto>
{
    public override string ToString()
    {
        string name = $"{nameof(GetUsersRequest)}";
        return name;
    }
}