namespace Infrastructure.PasswordHelps.Interfaces
{
    public interface IPasswordHash
    {
        string GetHashed();

        public class Fake : IPasswordHash
        {
            private readonly string _password;

            public Fake(string password)
            {
                _password = password;
            }
            public string GetHashed()
            {
                return _password + "Fake is hashed";
            }
        }
    }
}