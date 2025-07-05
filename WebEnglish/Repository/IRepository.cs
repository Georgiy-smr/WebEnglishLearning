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
            var status = new StatusGenericHandler()
            {
                Header = command.ToString(),
                Message = "I Fake status"
            };
            return Task.FromResult<IStatusGeneric>(status);
        }
        private static readonly Dictionary<Type, object> _samples = new()
        {
            { typeof(WordDto), new WordDto("hello", "привет", Id: 1, UserId: 42) },
            { typeof(UserDto), new UserDto("user1", "hash", Id: 1) }
        };

        public Task<IStatusGeneric<IEnumerable<TDto>>> GetItemsAsync<TDto>(IRequest<IStatusGeneric<IEnumerable<TDto>>> command, CancellationToken token = default) where TDto : BaseDto
        {
            var status = new StatusGenericHandler<IEnumerable<TDto>>()
            {
                Header = "Fake GetItems"
            };

            if (_samples.TryGetValue(typeof(TDto), out var sample))
            {
                var list = new List<TDto> { (TDto)sample };
                return Task.FromResult<IStatusGeneric<IEnumerable<TDto>>>(status.SetResult(list));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}