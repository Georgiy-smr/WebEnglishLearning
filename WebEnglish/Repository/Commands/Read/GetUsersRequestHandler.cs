using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.Commands.Read.ReadService;
using Repository.DTO;
using StatusGeneric;

namespace Repository.Commands.Read;

internal record GetUsersRequestHandler : IRequestHandler<GetUsersRequest, IStatusGeneric<IEnumerable<UserDto>>>
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<GetWordsRequestHandler> _logger;

    public GetUsersRequestHandler(IServiceProvider provider, ILogger<GetWordsRequestHandler> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task<IStatusGeneric<IEnumerable<UserDto>>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{request} Started");
        var status = new StatusGenericHandler<IEnumerable<UserDto>>();
        IEnumerable<UserDto> result = null!;
        await using var scope = _provider.CreateAsyncScope();
        try
        {
            var query =
                scope.ServiceProvider
                    .GetRequiredService<IEntityCollection<User>>().Get(request);

            if (!await query.AnyAsync(cancellationToken: cancellationToken))
            {
                status.AddError("items is empty");
                return status;
            }

            var list = await query
                .Select(x =>
                    new
                    {
                        n = x.UserName,
                        p = x.PasswordHash,
                        id = x.Id
                    }).ToListAsync(cancellationToken: cancellationToken);

            result = list
                .Select(x => new UserDto(x.n, x.p, Id: x.id));
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            status.AddError(e, e.Message);
        }
        _logger.LogInformation($"{request} Finish");
        return status.SetResult(result);
    }
}