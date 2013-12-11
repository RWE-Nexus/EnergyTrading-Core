namespace EnergyTrading.Crypto
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// Provides functionality to encrypt/decrypt using a pre-defined private key.
    /// </summary>
    public static class CryptoStringExtensions
    {
        private static readonly string Base64Key = "d9DgMhgII9dRHSSm+7XXENw6hYtHO2sh1+SdCQxtmrg=";

        private static readonly string Base64Iv = "7Z3BUqdrjArbNpE/uXpC0A==";

        private static readonly Random Random = new Random();

        /// <summary>
        /// Encrypt using our private key.
        /// </summary>
        /// <param name="plainText">Plain text to encrypt</param>
        /// <returns>Encrypted text.</returns>
        public static string EncryptString(this string plainText)
        {
            string encrypted;
            using (var crypto = SetupCryptograher())
            {
                var encryptor = crypto.CreateEncryptor();
                using (var ms = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cryptoStream))
                        {
                            var prefixed = GeneratePrefixString() + plainText;
                            writer.Write(prefixed);
                        }
                        encrypted = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return encrypted;
        }

        /// <summary>
        /// Replace encrypted password in the connection string with decrypted password
        /// </summary>
        /// <param name="connectionString">Connection string to use</param>
        /// <returns>Connection string with decrypted connection password</returns>
        public static string DecryptConnectionString(this string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("connectionString should not be empty or null");
            }

            var encryptedPassword = connectionString.Substring(connectionString.IndexOf(";Password=", StringComparison.InvariantCulture) + 10);
            var decryptedConnectionString = connectionString.Substring(0, connectionString.IndexOf(";Password=", StringComparison.InvariantCulture));
            var passwordSection = ";Password=" + encryptedPassword.DecryptString();
            decryptedConnectionString += passwordSection;

            return decryptedConnectionString;
        }

        /// <summary>
        /// Decrypt a string using our private key.
        /// </summary>
        /// <param name="encryptedText">Encrypted text to use.</param>
        /// <returns>Plain text</returns>
        public static string DecryptString(this string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                throw new ArgumentOutOfRangeException("encryptedText");
            }

            string plaintext;
            using (var crypto = SetupCryptograher())
            {
                var decryptor = crypto.CreateDecryptor();
                using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cryptoStream))
                        {
                            var prefixed = reader.ReadToEnd().TrimEnd('\0');
                            plaintext = prefixed.Substring(10);
                        }
                    }
                }
            }
            return plaintext;
        }

        private static RijndaelManaged SetupCryptograher()
        {
            var retVal = new RijndaelManaged
            {
                KeySize = 256,
                Key = Convert.FromBase64String(Base64Key),
                IV = Convert.FromBase64String(Base64Iv),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            };
            return retVal;
        }

        private static string GeneratePrefixString()
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[10];
            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = Chars[Random.Next(Chars.Length)];
            }
            return new string(stringChars);
        }
    }
}
