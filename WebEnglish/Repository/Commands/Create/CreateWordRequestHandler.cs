using ContextDataBase;
using Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StatusGeneric;

namespace Repository.Commands.Create;

public record CreateWordRequestHandler : IRequestHandler<CreateWordRequest, IStatusGeneric>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CreateWordRequestHandler> _logger;


    public CreateWordRequestHandler(IServiceProvider serviceProvider, ILogger<CreateWordRequestHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<IStatusGeneric> Handle(CreateWordRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{request} Started");
        StatusGenericHandler status = new StatusGenericHandler();

        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            await context!.AddAsync(new Word()
                {
                    EngWord = request.NewWord.Eng,
                    RusWord = request.NewWord.Rus,
                }, cancellationToken)
                .ConfigureAwait(false);

            await context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            status.AddError(e, e.Message);
        }
        _logger.LogInformation($"{request} Finish");
        return status;
    }
}