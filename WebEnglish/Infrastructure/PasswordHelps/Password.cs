using Infrastructure.PasswordHelps.Interfaces;

namespace Infrastructure.PasswordHelps
{
    public class Password : IPasswordHash
    {
        private readonly string _password;
        public Password(string password)
        {
            _password = password;
        }
        public string GetHashed() => BCrypt.Net.BCrypt.EnhancedHashPassword(_password);
    }
}
