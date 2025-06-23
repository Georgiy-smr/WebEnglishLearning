using Infrastructure.PasswordHelps.Interfaces;

namespace Infrastructure.PasswordHelps;

public class HashValidate : IValidatePassword
{
    private readonly string _passwordHashed;
    public HashValidate(string passwordHashed)
    {
        _passwordHashed = passwordHashed;
    }
    public bool Validate(string password) => BCrypt.Net.BCrypt.EnhancedVerify(password, _passwordHashed);
}