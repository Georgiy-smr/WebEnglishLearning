using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataBaseServices;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Commands.Read;
using Repository.DTO;
using StatusGeneric;

namespace DataBaseTests
{
    public class RepositoryTests
    {
        [Fact]
        public async void GetWordsDto()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDataBaseServices("testRepository.db");
            
            IServiceProvider provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();

            bool resultInitialize = await scope.ServiceProvider.GetRequiredService<Initialization>().InitializeAsync();

            if (!resultInitialize)
                throw new Exception("DataBase is not initialized");

            //Arrange

            var sut = scope.ServiceProvider.GetRequiredService<IRepository>();

            GetWordsRequest readCommand = new GetWordsRequest()
            {
                Filters = new List<Expression<Func<Word, bool>>>(),
                Includes = new List<Expression<Func<Word, object>>>(),
                Size = 4,
                ZeroStart = 0
            };

            //Act

            IStatusGeneric<IEnumerable<WordDto>> resultCollection = await sut.GetItemsAsync<WordDto>(readCommand);

            //Assert

            Assert.True(resultCollection.IsValid);
            Assert.True(resultCollection.Result.Any());
            Assert.True(resultCollection.Result.Count() == 4);
        }

    }
}
