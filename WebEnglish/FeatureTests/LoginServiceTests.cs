using DataBaseTests;
using Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace FeatureTests
{
    public class LoginServiceTests : Di
    {
        protected override IServiceCollection SutServices
        {
            get
            {
                var services = new ServiceCollection();
                services.AddScoped<IAuthService, IAuthService.Fake>();
                services.AddScoped<IRepository, IRepository.Fake>();
                services.AddScoped<IGenerateToken, IGenerateToken.Fake>();

                services.AddScoped<ILogin, UserService>();

                return services;
            }
        }


        [Fact]
        public async void Test1()
        {
            using IServiceScope scope = ServicesProvider.CreateScope();
            IServiceProvider providerInScope = scope.ServiceProvider;

            ILogin sut = providerInScope.GetRequiredService<ILogin>();

            await sut.Register("Gosha", "1234");

        }


    }
}