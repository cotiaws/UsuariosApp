using System.Security.Cryptography;
using System.Text;

namespace UsuariosApp.API.Helpers
{
    public static class CryptoHelper
    {
        public static string GetSHA256(string value)
        {
            using var sha256 = SHA256.Create();

            var inputBytes = Encoding.UTF8.GetBytes(value);
            var hashBytes = sha256.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}
