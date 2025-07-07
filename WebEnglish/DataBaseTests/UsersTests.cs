using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ContextDataBase;
using DataBaseServices;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Commands.Create;
using Repository.Commands.Read;
using Repository.DTO;
using Repository;
using Repository.Commands.Create.Users;
using Repository.Commands.Read.Users;
using StatusGeneric;

namespace DataBaseTests
{
    public class UsersTests : Di
    {
        protected override IServiceCollection SutServices =>
            new ServiceCollection()
                .AddLogging()
                .AddDbContext<AppDbContext>((options) =>
                {
                    options.UseSqlite($"Data Source=UserTests.db");
                })
                .AddScoped<Initialization>()
                .AddRepository()
        ;


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

            string newName = "topName" + Random.Shared.NextInt64(long.MaxValue);


            CreateUserRequest createUserRequestCreateCommand = new CreateUserRequest(new UserDto(newName, "2308"));

            //Act

            IStatusGeneric resultAddedNewUser = await sut.DataBaseOperationAsync(createUserRequestCreateCommand);

            //Assert

            Assert.True(resultAddedNewUser.IsValid);


            GetUsersRequest readCommand = new GetUsersRequest
            {
                Filters = new List<Expression<Func<User, bool>>>()
                {
                    x => x.UserName == newName && x.PasswordHash == "2308"
                },
                Includes = new List<Expression<Func<User, object>>>(),
                Size = 1,
                ZeroStart = 0
            };
            IStatusGeneric<IEnumerable<UserDto>> resultRead = await sut.GetItemsAsync<UserDto>(readCommand);

            Assert.True(resultRead.IsValid);
            Assert.NotNull(resultRead.Result.Single());
        }

        [Fact]
        public async void Read()
        {

            IServiceProvider provider = SutServices.BuildServiceProvider();


            bool resultInitialize = await provider.GetRequiredService<Initialization>().InitializeAsync();

            if (!resultInitialize)
                throw new Exception("DataBase is not initialized");

            //Arrange

            var sut = provider.GetRequiredService<IRepository>();

            GetUsersRequest readCommand = new GetUsersRequest()
            {
                Filters = new List<Expression<Func<User, bool>>>(),
                Includes = new List<Expression<Func<User, object>>>(),
                Size = 4,
                ZeroStart = 0
            };

            //Act

            IStatusGeneric<IEnumerable<UserDto>> resultCollection = await sut.GetItemsAsync<UserDto>(readCommand);

            //Assert

            Assert.True(resultCollection.IsValid);
            Assert.True(resultCollection.Result.Any());
            Assert.True(resultCollection.Result.Count() == 4);
        }
    }
}
