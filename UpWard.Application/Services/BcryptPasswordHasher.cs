using BCrypt.Net;
using Upward.Application.Interfaces.IService;

namespace Upward.Application.Services
{    
        public class BcryptPasswordHasher : IPasswordHasher
        {
            public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

            public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    
        }

}