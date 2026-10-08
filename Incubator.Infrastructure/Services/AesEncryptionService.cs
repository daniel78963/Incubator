using Incubator.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Incubator.Infrastructure.Services
{
    public class AesEncryptionService : IEncryptionService
    {
        // En producción, estas claves deben venir de appsettings.json o Azure Key Vault.
        // Key debe ser de 32 bytes (256 bits) y el IV de 16 bytes.
        private readonly byte[] _key = Encoding.UTF8.GetBytes("IncubatorSecretKey12345678901234");
        private readonly byte[] _iv = Encoding.UTF8.GetBytes("VectorDe16Bytes!");

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
