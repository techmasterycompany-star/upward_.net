using BCrypt.Net;
using Upwork.Application.Interfaces.IService;

namespace Upwork.Application.Services
{    
        public class BcryptPasswordHasher : IPasswordHasher
        {
            public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

            public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    
        }

}