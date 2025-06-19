using ContextDataBase;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.DTO;
using StatusGeneric;

namespace Repository.Commands.Delete;

public record BaseDeleteCommand(int IdEntity) : BaseCommandDataBase();

public record DeleteWordRequest(int Id) : BaseDeleteCommand(Id)
{
    public DeleteWordRequest(BaseDto wordToRemove) : this(wordToRemove.Id) {}
    public DeleteWordRequest(IEntity entityToRemove) : this(entityToRemove.Id) { }
    public override string ToString()
    {
        string name = $"{nameof(DeleteWordRequest)} {Id}";
        return base.ToString();
    }
}

public record DeleteWordRequestHandler : IRequestHandler<DeleteWordRequest, IStatusGeneric>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DeleteWordRequestHandler> _logger;

    public DeleteWordRequestHandler(IServiceProvider provider, ILogger<DeleteWordRequestHandler> logger)
    {
        _serviceProvider = provider;
        _logger = logger;
    }

    public async Task<IStatusGeneric> Handle(DeleteWordRequest request, CancellationToken cancellationToken)
    {
        string msg = $"{nameof(DeleteWordRequestHandler)} {request}";
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
            var toRemove = await context.Words.SingleAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            toRemove.SoftDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"{request} is success");
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            status.AddError(e, e.Message);
        }
        return status;
    }
}