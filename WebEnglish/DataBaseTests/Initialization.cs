using ContextDataBase;
using DataBaseServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataBaseTests
{
    public class InitializationTest : Di
    {
        protected override IServiceCollection SutServices =>
            new ServiceCollection()
                .AddLogging()
                .AddDbContext<AppDbContext>((options) =>
                {
                    options.UseSqlite($"Data Source=TestInitDataBase.db");
                })
                .AddScoped<Initialization>()
            ;

        [Fact]
        public async void TestResultTrue()
        {
            IServiceProvider serviceProvider = SutServices.BuildServiceProvider();

            var init = serviceProvider.GetRequiredService<Initialization>();
            var result = await init.InitializeAsync();
            Assert.True(result);
        }

    }
    
}