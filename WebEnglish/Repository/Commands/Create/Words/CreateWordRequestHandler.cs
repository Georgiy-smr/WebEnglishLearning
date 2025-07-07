using ContextDataBase;
using Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StatusGeneric;

namespace Repository.Commands.Create.Words;

public record CreateWordRequestHandler : IRequestHandler<CreateWordRequest, IStatusGeneric>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CreateWordRequestHandler> _logger;


    public CreateWordRequestHandler(IServiceProvider serviceProvider, ILogger<CreateWordRequestHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<IStatusGeneric> Handle(CreateWordRequest createWordRequest, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{createWordRequest} Started");
        StatusGenericHandler status = new StatusGenericHandler();

        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            User? user = context.Users.FirstOrDefault(x => x.Id == createWordRequest.NewWord.UserId);

            await context!.AddAsync(new Word()
                {
                    EngWord = createWordRequest.NewWord.Eng,
                    RusWord = createWordRequest.NewWord.Rus,
                    User = user
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
        _logger.LogInformation($"{createWordRequest} Finish");
        return status;
    }
}