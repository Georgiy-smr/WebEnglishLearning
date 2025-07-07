using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.Commands.Read.ReadService;
using Repository.DTO;
using StatusGeneric;

namespace Repository.Commands.Read.Words;

internal record GetWordsRequestHandler : IRequestHandler<GetWordsRequest, IStatusGeneric<IEnumerable<WordDto>>>
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<GetWordsRequestHandler> _logger;

    public GetWordsRequestHandler(IServiceProvider provider, ILogger<GetWordsRequestHandler> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task<IStatusGeneric<IEnumerable<WordDto>>> Handle(GetWordsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{request} Started");
        var status = new StatusGenericHandler<IEnumerable<WordDto>>();
        IEnumerable<WordDto> result = null!;
        await using var scope = _provider.CreateAsyncScope();
        try
        {
            var query =
                scope.ServiceProvider
                    .GetRequiredService<IEntityCollection<Word>>().Get(request);

            if (!await query.AnyAsync(cancellationToken: cancellationToken))
            {
                status.AddError("items is empty");
                return status;
            }

            var list = await query
                .Select(x =>
                    new
                    {
                        eng = x.EngWord,
                        rus = x.RusWord,
                        id = x.Id
                    }).ToListAsync(cancellationToken: cancellationToken);

            result = list
                .Select(x => new WordDto(x.eng,x.rus, Id: x.id));
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