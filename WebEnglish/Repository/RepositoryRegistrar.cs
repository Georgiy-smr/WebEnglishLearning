using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Repository.Commands.Read.ReadService;

namespace Repository
{
    public static class RepositoryRegistrar
    {
        public static IServiceCollection AddRepository(this IServiceCollection serviceCollection)
        {
           return serviceCollection
               .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
               .AddScoped(typeof(IEntityCollection<>), typeof(DataBaseEntitiyes<>))
               .AddScoped<IRepository, Repository>()
               ;
        } 
    }
}
