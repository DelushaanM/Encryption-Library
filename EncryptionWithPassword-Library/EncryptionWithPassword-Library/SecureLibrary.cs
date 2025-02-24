using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecureLibrary
{
    public class Encryptor
    {
        private const int KeySize = 32; // 256 bits
        private const int SaltSize = 16; // 128 bits
        private const int IvSize = 16; // 128 bits
        private const int Iterations = 100000; // PBKDF2 iterations

        public static string Encrypt(string plainText, string password)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                var salt = GenerateRandomBytes(SaltSize);
                var iv = GenerateRandomBytes(IvSize);
                aes.Key = DeriveKeyFromPassword(password, salt);
                aes.IV = iv;

                using (var ms = new MemoryStream())
                {
                    ms.Write(salt, 0, salt.Length);
                    ms.Write(iv, 0, iv.Length);

                    using (var encryptor = aes.CreateEncryptor())
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string encryptedText, string password)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);

            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var ms = new MemoryStream(encryptedBytes))
                {
                    var salt = new byte[SaltSize];
                    ms.Read(salt, 0, SaltSize);
                    var iv = new byte[IvSize];
                    ms.Read(iv, 0, IvSize);

                    aes.Key = DeriveKeyFromPassword(password, salt);
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor())
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        private static byte[] DeriveKeyFromPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(KeySize);
            }
        }

        private static byte[] GenerateRandomBytes(int size)
        {
            var randomBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
