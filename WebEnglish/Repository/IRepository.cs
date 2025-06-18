using MediatR;
using Repository.Commands;
using Repository.DTO;
using StatusGeneric;

namespace Repository;

public interface IRepository
{
    Task<IStatusGeneric> DataBaseOperationAsync(
        BaseCommandDataBase command,
        CancellationToken token = default);

    Task<IStatusGeneric<IEnumerable<TDto>>> GetItemsAsync<TDto>(
        IRequest<IStatusGeneric<IEnumerable<TDto>>> command, CancellationToken token = default)
        where TDto : BaseDto;
}