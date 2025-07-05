namespace Infrastructure.PasswordHelps.Interfaces;

public interface IValidatePassword
{
    bool Validate(string password);

    public class Fake : IValidatePassword
    {
        public bool Validate(string password)
        {
            return true;
        }
    }


}