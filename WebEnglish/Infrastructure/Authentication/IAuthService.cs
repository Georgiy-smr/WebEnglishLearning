using Infrastructure.PasswordHelps;
using Infrastructure.PasswordHelps.Interfaces;

namespace Infrastructure.Authentication;

public interface IAuthService
{
    IPasswordHash Hash(string source);
    IValidatePassword CreateValidating(string hashed);
}

public class PasswordHash : IAuthService
{
    public IPasswordHash Hash(string source) => new Password(source);

    public IValidatePassword CreateValidating(string hashed) =>
        new HashValidateWithException(
            new HashValidate(hashed));
}
