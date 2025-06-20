using System.Reflection;
using ContextDataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repository;
using Repository.Commands.Read.ReadService;

namespace DataBaseServices
{
    public static class Registration
    {
        public static IServiceCollection AddDataBaseServices(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddLogging()
                .AddSingleton<BdSettings>()
                .AddDbContext<AppDbContext>((servicesProvider, options) =>
                {
                    var settings = servicesProvider.GetRequiredService<IOptions<BdSettings>>().Value;
                    var connectionStrings = settings.ConnectionStrings!.SQLite;
                    options.UseSqlite($"Data Source={connectionStrings}");
                })
                .AddScoped<Initialization>()
                .AddRepository()
        ;
    }
}
