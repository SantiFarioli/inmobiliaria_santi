using System.Security.Cryptography;
using System.Text;

namespace inmobiliaria_santi.Models
{
    public static class HashHelper
    {
        public static string CalcularHash(string texto)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(texto);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
