using Infrastructure.Authentication;
using Repository;

namespace LogInTests
{
    public class RegisterTests
    {
        [Fact]
        public async void RegisterResultTrue()
        {
            ILogin sut = new UserService(
                new IAuthService.Fake(),
                new IRepository.Fake(),
                new IGenerateToken.Fake());
            var result = await sut.Register("Gosha", "Password");
            Assert.True(result.IsValid);
        }
    }
}