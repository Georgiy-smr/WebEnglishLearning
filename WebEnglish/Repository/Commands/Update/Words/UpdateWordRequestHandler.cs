using ContextDataBase;
using Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.DTO;
using StatusGeneric;

namespace Repository.Commands.Update.Words;

public record UpdateWordRequestHandler : IRequestHandler<RequestUpdateWordRequest, IStatusGeneric>
{
    private readonly IServiceProvider _serviceProvider;
    ILogger<UpdateWordRequestHandler> _logger;

    public UpdateWordRequestHandler(IServiceProvider serviceProvider, ILogger<UpdateWordRequestHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }


    public async Task<IStatusGeneric> Handle(RequestUpdateWordRequest request, CancellationToken cancellationToken)
    {
        string msg = $"{nameof(UpdateWordRequestHandler)} {request}";
        _logger.LogInformation(msg);
        StatusGenericHandler status = new StatusGenericHandler()
        {
            Header = msg
        };
        await using var scope = _serviceProvider.CreateAsyncScope();
        IServiceProvider providerInScope = scope.ServiceProvider;


        try
        {
            AppDbContext context = providerInScope.GetRequiredService<AppDbContext>();

            Word originalWord = context.Words.Single(x => x.Id.Equals(request.Modified.Id));

            ChangeWord(original: ref originalWord, request.Modified);

            await context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Success");
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            status.AddError(e, e.Message);
        }
        return status;
    }


    private void ChangeWord(ref Word original, WordDto modifiedDto)
    {
        original.EngWord = modifiedDto.Eng;
        original.RusWord = modifiedDto.Rus;
    }

}