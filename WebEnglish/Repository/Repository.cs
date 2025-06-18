using MediatR;
using Repository.Commands;
using Repository.DTO;
using StatusGeneric;

namespace Repository;

public class Repository : IRepository
{
    private readonly IMediator _mediator;
    public Repository(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<IStatusGeneric<IEnumerable<TDto>>> GetItemsAsync<TDto>(
        IRequest<IStatusGeneric<IEnumerable<TDto>>> command, CancellationToken token = default)
        where TDto : BaseDto
    {
        return _mediator.Send(command, token);
    }

    public Task<IStatusGeneric> DataBaseOperationAsync(
        BaseCommandDataBase command,
        CancellationToken token = default)
    {
        return _mediator.Send(command, token);
    }

}