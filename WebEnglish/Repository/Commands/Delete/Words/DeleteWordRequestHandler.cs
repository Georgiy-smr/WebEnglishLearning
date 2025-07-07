using ContextDataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StatusGeneric;

namespace Repository.Commands.Delete.Words;

/// <inheritdoc />
public record DeleteWordRequestHandler : IRequestHandler<DeleteWordRequest, IStatusGeneric>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DeleteWordRequestHandler> _logger;

    public DeleteWordRequestHandler(IServiceProvider provider, ILogger<DeleteWordRequestHandler> logger)
    {
        _serviceProvider = provider;
        _logger = logger;
    }

    public async Task<IStatusGeneric> Handle(DeleteWordRequest deleteWordRequest, CancellationToken cancellationToken)
    {
        string msg = $"{nameof(DeleteWordRequestHandler)} {deleteWordRequest}";
        _logger.LogInformation($"{msg}");
        StatusGenericHandler status = new StatusGenericHandler()
        {
            Header = msg
        };
        await using var scope = _serviceProvider.CreateAsyncScope();
        var provider = scope.ServiceProvider;
        try
        {
            var context = provider.GetRequiredService<AppDbContext>();
            var toRemove = await context.Words.SingleAsync(x => x.Id == deleteWordRequest.Id, cancellationToken: cancellationToken);
            toRemove.SoftDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"{deleteWordRequest} is success");
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            status.AddError(e, e.Message);
        }
        return status;
    }
}