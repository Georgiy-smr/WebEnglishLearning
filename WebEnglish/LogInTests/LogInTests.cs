using Infrastructure.Authentication;
using Repository;

namespace LogInTests;

public class LogInTests
{
    [Fact]
    public async void ResultTrue()
    {
        ILogin sut = new UserService(
            new IAuthService.Fake(),
            new IRepository.Fake(),
            new IGenerateToken.Fake());
        var result = await sut.CreateToken("Gosha", "Password");
        Assert.True(result.IsValid);
    }
}