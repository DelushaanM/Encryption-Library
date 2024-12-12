using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecureLibrary
{
    public class Encryptor
    {
        // Method to encrypt a value using AES and a password
        public static string Encrypt(string plainText, string password)
        {
            using (var aes = Aes.Create())
            {
                var key = GenerateKey(password, aes.KeySize / 8);
                aes.Key = key;
                aes.GenerateIV(); // Generate a random Initialization Vector (IV)

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    // Write IV at the beginning for decryption
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        // Method to decrypt a value using AES and a password
        public static string Decrypt(string encryptedText, string password)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);

            using (var aes = Aes.Create())
            {
                var key = GenerateKey(password, aes.KeySize / 8);
                aes.Key = key;

                using (var ms = new MemoryStream(encryptedBytes))
                {
                    // Read the IV from the stream
                    var iv = new byte[aes.BlockSize / 8];
                    ms.Read(iv, 0, iv.Length);
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        // Helper method to generate a key from the password
        private static byte[] GenerateKey(string password, int keySize)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(passwordBytes);

                // Ensure the key size matches the AES key size
                var key = new byte[keySize];
                Array.Copy(hash, key, keySize);

                return key;
            }
        }
    }
}
