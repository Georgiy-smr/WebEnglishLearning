using Infrastructure.PasswordHelps.Interfaces;

namespace Infrastructure.PasswordHelps;

public class HashValidateWithException : IValidatePassword
{
    private readonly IValidatePassword _validatePassword;

    public HashValidateWithException(IValidatePassword validatePassword)
    {
        _validatePassword = validatePassword;
    }
    public bool Validate(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));
        return _validatePassword.Validate(password);
    }
}