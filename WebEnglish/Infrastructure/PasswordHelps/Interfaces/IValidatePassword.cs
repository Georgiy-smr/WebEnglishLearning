namespace Infrastructure.PasswordHelps.Interfaces;

public interface IValidatePassword
{
    bool Validate(string password);
}