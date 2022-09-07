using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Api.Services.Hashing
{
    public class HashProvider : IHashProvider
    {
        public virtual byte[] ComputeHash(string data, Encoding encoding, out byte[] salt)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException($"Argument {data} cannot be null.");

            if (encoding is null)
                throw new ArgumentNullException($"Argument {encoding} cannot be null.");

            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            return hmac.ComputeHash(encoding.GetBytes(data));
        }

        public virtual byte[] ComputeHash(string data, Encoding encoding, byte[] salt)
        {
            if (salt is null || salt.Length == 0)
                throw new ArgumentNullException($"Argument {salt} cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException($"Argument {data} cannot be null.");

            if (encoding is null)
                throw new ArgumentNullException($"Argument {encoding} cannot be null.");

            using var hmac = new HMACSHA512(salt);

            return hmac.ComputeHash(encoding.GetBytes(data));
        }

        public virtual byte[] ComputeUtf8EncodedHash(string data, out byte[] salt)
            => ComputeHash(data, Encoding.UTF8, out salt);

        public virtual byte[] ComputeUtf8EncodedHash(string data, byte[] salt)
            => ComputeHash(data, Encoding.UTF8, salt);
    }
}