using Infrastructure.PasswordHelps;
using Infrastructure.PasswordHelps.Interfaces;

namespace Infrastructure.Authentication;

public interface IAuthService
{
    IPasswordHash Hash(string source);
    IValidatePassword CreateValidating(string hashed);

    public class Fake : IAuthService
    {
        public IPasswordHash Hash(string source)
        {
            var fakeHash = new IPasswordHash.Fake(source);
            return fakeHash;
        }

        public IValidatePassword CreateValidating(string hashed)
        {
            throw new NotImplementedException();
        }
    }


}

public class PasswordHash : IAuthService
{
    public IPasswordHash Hash(string source) => new Password(source);

    public IValidatePassword CreateValidating(string hashed) =>
        new HashValidateWithException(
            new HashValidate(hashed));
}
