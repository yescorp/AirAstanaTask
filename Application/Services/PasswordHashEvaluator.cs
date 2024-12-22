using Application.Abstractions.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PasswordHashEvaluator : IPasswordHashEvaluator
    {
        public (string password, string saltBase64) HashPassword(string password)
        {
            var (salt, saltBase64) = GenerateSalt();
            string hashed = GenerateHash(password, salt);
            return (hashed, saltBase64);
        }

        public bool PasswordMatch(string password, string saltBase64, string hash)
        {
            var salt = Convert.FromBase64String(saltBase64);
            var generatedHash = GenerateHash(password, salt);

            return hash == generatedHash;
        }
        
        private string GenerateHash(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }
        private (byte[] salt, string saltBase64) GenerateSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            var saltBase64 = Convert.ToBase64String(salt);
            var saltFromString = Convert.FromBase64String(saltBase64);

            return (saltFromString, saltBase64);
        }
    }
}
