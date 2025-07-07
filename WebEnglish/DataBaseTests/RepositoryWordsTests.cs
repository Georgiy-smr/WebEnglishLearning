using System.Linq.Expressions;
using ContextDataBase;
using DataBaseServices;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Commands.Create;
using Repository.Commands.Delete;
using Repository.Commands.Read;
using Repository.Commands.Update;
using Repository.DTO;
using StatusGeneric;

namespace DataBaseTests
{
    public class RepositoryWordsTests : Di
    {
        protected override IServiceCollection SutServices =>
            new ServiceCollection()
                .AddLogging()
                .AddDbContext<AppDbContext>((options) =>
                {
                    options.UseSqlite($"Data Source=TestInitDataBase.db");
                })
                .AddScoped<Initialization>()
                .AddRepository()
        ;


        [Fact]
        public async void Read()
        {

            IServiceProvider provider = SutServices.BuildServiceProvider();


            bool resultInitialize = await provider.GetRequiredService<Initialization>().InitializeAsync();

            if (!resultInitialize)
                throw new Exception("DataBase is not initialized");

            //Arrange

            var sut = provider.GetRequiredService<IRepository>();

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

        [Fact]
        public async void Create()
        {
            
            IServiceProvider provider = SutServices.BuildServiceProvider();

            using var scope = provider.CreateScope();

            bool resultInitialize = await scope.ServiceProvider.GetRequiredService<Initialization>().InitializeAsync();

            if (!resultInitialize)
                throw new Exception("DataBase is not initialized");

            //Arrange

            var sut = scope.ServiceProvider.GetRequiredService<IRepository>();

            RequestCreateWordRequest requestCreateCommand = new RequestCreateWordRequest(new WordDto("Mom", "Мама"));

            //Act

            IStatusGeneric resultCollection = await sut.DataBaseOperationAsync(requestCreateCommand);

            //Assert
            
            Assert.True(resultCollection.IsValid);


            GetWordsRequest readCommand = new GetWordsRequest
            {
                Filters = new List<Expression<Func<Word, bool>>>()
                {
                    x => x.EngWord == "Mom",
                    x => x.RusWord == "Мама"
                },
                Includes = new List<Expression<Func<Word, object>>>(),
                Size = 1,
                ZeroStart = 0
            };
            IStatusGeneric<IEnumerable<WordDto>> resultRead = await sut.GetItemsAsync<WordDto>(readCommand);

            Assert.True(resultRead.IsValid);
            Assert.NotNull(resultRead.Result.Single());
        }



        [Fact]
        public async void Update()
        {
            IServiceProvider provider = SutServices.BuildServiceProvider();

            using var scope = provider.CreateScope();

            bool resultInitialize = await scope.ServiceProvider.GetRequiredService<Initialization>().InitializeAsync();

            if (!resultInitialize)
                throw new Exception("DataBase is not initialized");


            //Arrange

            var sut = scope.ServiceProvider.GetRequiredService<IRepository>();
            GetWordsRequest readSingle = new GetWordsRequest
            {
                Filters = new List<Expression<Func<Word, bool>>>()
                {

                },
                Includes = new List<Expression<Func<Word, object>>>()
                {

                },
                Size = 10,
                ZeroStart = 0
            };
            IStatusGeneric<IEnumerable<WordDto>> resultRead = await sut.GetItemsAsync<WordDto>(readSingle);
            Assert.True(resultRead.IsValid);
            WordDto oldWord = resultRead.Result.Last();

            //Act

            RequestUpdateWordRequest requestUpdateCommand = new RequestUpdateWordRequest(oldWord with { Rus = "Привет!", Eng = "Hello!" });

            IStatusGeneric resultUpdate = await sut.DataBaseOperationAsync(requestUpdateCommand);

            Assert.True(resultUpdate.IsValid);

            //Assert

            GetWordsRequest readUpdateSingle = new GetWordsRequest
            {
                Filters = new List<Expression<Func<Word, bool>>>()
                {
                    x => x.Id == oldWord.Id,
                    x => x.RusWord == "Привет!",
                    x => x.EngWord ==  "Hello!"
                },
                Includes = new List<Expression<Func<Word, object>>>()
                {

                },
                Size = 1,
                ZeroStart = 0
            };

            IStatusGeneric<IEnumerable<WordDto>> resultReadNewWord = await sut.GetItemsAsync(readUpdateSingle);

            Assert.True(resultReadNewWord.IsValid);
        }

        [Fact]
        public async void Delete()
        {
            IServiceProvider provider = SutServices.BuildServiceProvider();

            using var scope = provider.CreateScope();

            bool resultInitialize = await scope.ServiceProvider.GetRequiredService<Initialization>().InitializeAsync();

            if (!resultInitialize)
                throw new Exception("DataBase is not initialized");


            //Arrange

            var sut = scope.ServiceProvider.GetRequiredService<IRepository>();
            GetWordsRequest readSingle = new GetWordsRequest
            {
                Filters = new List<Expression<Func<Word, bool>>>()
                {

                },
                Includes = new List<Expression<Func<Word, object>>>()
                {

                },
                Size = 10,
                ZeroStart = 0
            };
            IStatusGeneric<IEnumerable<WordDto>> resultRead = await sut.GetItemsAsync<WordDto>(readSingle);
            Assert.True(resultRead.IsValid);
            WordDto toRemove = resultRead.Result.Last();

            //Act

            RequestDeleteWordRequest requestDeleteCommand = new RequestDeleteWordRequest(toRemove);

            IStatusGeneric resultUpdate = await sut.DataBaseOperationAsync(requestDeleteCommand);

            Assert.True(resultUpdate.IsValid);

            //Assert

            GetWordsRequest readRemovedWordCommand = new GetWordsRequest
            {
                Filters = new List<Expression<Func<Word, bool>>>()
                {
                    x => x.Id == toRemove.Id,
                },
                Includes = new List<Expression<Func<Word, object>>>()
                {

                },
                Size = 1,
                ZeroStart = 0
            };

            var resultReadRemoved = await sut.GetItemsAsync(readRemovedWordCommand);

            Assert.False(resultReadRemoved.IsValid);
        }


    }
}
