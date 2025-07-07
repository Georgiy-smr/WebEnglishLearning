using DataBaseOperationHelper.BaseCommandsOperations.Reading;
using Entities;
using MediatR;
using Repository.DTO;
using StatusGeneric;

namespace Repository.Commands.Read;

public record GetUsersRequest() : 
        ReadCommandRequest<User,UserDto>
{
    public override string ToString()
    {
        string name = $"{nameof(GetUsersRequest)}";
        return name;
    }
}