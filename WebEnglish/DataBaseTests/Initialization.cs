using DataBaseServices;
using Microsoft.Extensions.DependencyInjection;

namespace DataBaseTests
{
    public class InitializationTest
    {
        [Fact]
        public async void TestResultTrue()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddDataBaseServices("testData.db");
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var init = serviceProvider.GetRequiredService<Initialization>();
            var result = await init.InitializeAsync();
            Assert.True(result);
        }
    }
    
}