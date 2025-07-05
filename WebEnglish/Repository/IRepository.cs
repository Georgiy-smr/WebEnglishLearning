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

    public class Fake : IRepository
    {
        public Task<IStatusGeneric> DataBaseOperationAsync(BaseCommandDataBase command, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<IStatusGeneric<IEnumerable<TDto>>> GetItemsAsync<TDto>(IRequest<IStatusGeneric<IEnumerable<TDto>>> command, CancellationToken token = default) where TDto : BaseDto
        {
            throw new NotImplementedException();
        }
    }


}