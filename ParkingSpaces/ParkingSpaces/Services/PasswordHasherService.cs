using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Cryptography;

namespace ParkingSpaces.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private const int SaltSize = 128 / 8; // 16 bytes
        private const int KeySize = 256 / 8; // 32 bytes

        //private const int KeySize = 256 / 16; // 16 bytes

        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private static char Delimiter = ';';

        public string Hash(string password)
        {
            // 1 byte - 8 bits -> 0 to 255 decimal num
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize); // additional security
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

            // encode the bytes into strings using Base64 -> (binary to text encoding)
            // base64 - represent binary data with possible of 64 chars
            string saltStirng = Convert.ToBase64String(salt);
            string hashString = Convert.ToBase64String(hash);

            return string.Join(Delimiter, saltStirng, hashString);
        }

        public bool Verify(string passwordHash, string inputPassword)
        {
            string[] elements = passwordHash.Split(Delimiter);
            byte[] salt = Convert.FromBase64String(elements[0]);
            byte[] hash = Convert.FromBase64String(elements[1]);

            // with the same salt!
            byte[] hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithmName, KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, hashInput);
        }
    }
}
