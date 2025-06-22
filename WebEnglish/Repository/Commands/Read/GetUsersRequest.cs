using Entities;
using MediatR;
using Repository.DTO;
using StatusGeneric;

namespace Repository.Commands.Read;

public record GetUsersRequest() :
    ReadCommand<User>,
    IRequest<IStatusGeneric<IEnumerable<UserDto>>>
{
    public override string ToString()
    {
        string name = $"{nameof(GetUsersRequest)}";
        return name;
    }
}