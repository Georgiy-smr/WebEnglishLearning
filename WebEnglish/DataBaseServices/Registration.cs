using System.Reflection;
using ContextDataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Commands.Read.ReadService;

namespace DataBaseServices
{
    public static class Registration
    {
        public static IServiceCollection AddDataBaseServices(this IServiceCollection serviceCollection, string pathFileDataBase) =>
            serviceCollection
                .AddLogging()
                .AddSingleton<Settings>()
                .AddDbContext<AppDbContext>((servicesProvider, options) =>
                {
                    if (string.IsNullOrWhiteSpace(pathFileDataBase))
                        pathFileDataBase = servicesProvider.GetRequiredService<Settings>().ToString();
                    options.UseSqlite($"Data Source={pathFileDataBase}");
                })
                .AddSingleton<Initialization>()
                .AddRepository()
        ;
    }
}
