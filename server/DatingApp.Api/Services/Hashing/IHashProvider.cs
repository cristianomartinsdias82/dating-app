using System.Text;

namespace DatingApp.Api.Services.Hashing
{
    public interface IHashProvider
    {
         byte[] ComputeHash(string data, Encoding encoding, out byte[] salt);
         byte[] ComputeHash(string data, Encoding encoding, byte[] salt);
         byte[] ComputeUtf8EncodedHash(string data, out byte[] salt);
         byte[] ComputeUtf8EncodedHash(string data, byte[] salt);
    }
}