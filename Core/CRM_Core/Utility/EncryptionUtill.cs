using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.Utility
{
    public sealed class EncryptionUtill : IEncryptionUtill
    {
        #region Fields
        private readonly AppSettings _appSettings;
        private readonly byte[] _salt_Bytes;
        #endregion
        #region Contructors
        public EncryptionUtill(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            if (!String.IsNullOrEmpty(_appSettings.EncSalt))
                _salt_Bytes = Encoding.UTF8.GetBytes(_appSettings.EncSalt);
            else
                _salt_Bytes = new byte[] { 12, 21, 13, 31, 36, 78, 81, 25, 1, 23, 67, 33, 71, 5, 12, 14 };
        }
        #endregion
        #region Encryption AES


        public string EncryptString_Aes(string plainText)
        {
            // Check arguments.
            if (String.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (String.IsNullOrEmpty(_appSettings.EncKey))
                throw new ArgumentNullException("Key");

            byte[] encryptedBytes;
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(_appSettings.EncKey.Substring(0, 16));

            using (MemoryStream ms = new MemoryStream())
            {
                using (var _aes = AesCng.Create())
                {
                    _aes.KeySize = 256;
                    _aes.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, _salt_Bytes, 1000);
                    _aes.Key = key.GetBytes(_aes.KeySize / 8);
                    _aes.IV = key.GetBytes(_aes.BlockSize / 8);

                    using (CryptoStream csEncrypt = new CryptoStream(ms, _aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encryptedBytes = ms.ToArray();
                    }
                    
                }
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        public string DecryptString_Aes(string encryptedText)
        {
            // Check arguments.
            if (String.IsNullOrEmpty(encryptedText))
                throw new ArgumentNullException("encryptedText");
            if (String.IsNullOrEmpty(_appSettings.EncKey))
                throw new ArgumentNullException("Key");

            // Declare the string used to hold
            // the decrypted text.
            byte[] decryptedBytes;

            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(_appSettings.EncKey.Substring(0, 16));

            using (MemoryStream ms = new MemoryStream())
            {
                using (var _aes = AesCng.Create())
                {
                    _aes.KeySize = 256;
                    _aes.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, _salt_Bytes, 1000);
                    _aes.Key = key.GetBytes(_aes.KeySize / 8);
                    _aes.IV = key.GetBytes(_aes.BlockSize / 8);

                    using (CryptoStream csEncrypt = new CryptoStream(ms, _aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(encryptedText);
                        }
                        decryptedBytes = ms.ToArray();
                    }

                }
            }
            return Convert.ToBase64String(decryptedBytes);
        }

        public string AES_Encrypt(string plainText)
        {
            if (String.IsNullOrEmpty(plainText))
                return null;
            if (String.IsNullOrEmpty(_appSettings.EncKey))
                throw new NullReferenceException("key is required");
            byte[] _aES_Password_Bytes = System.Text.Encoding.UTF8.GetBytes(_appSettings.EncKey.Substring(0, 16));
            byte[] passwordBytes = SHA512.Create().ComputeHash(_aES_Password_Bytes);
            byte[] originalBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = null;

            // Getting the salt size
            int saltSize = GetSaltSize(passwordBytes, _salt_Bytes);
            // Generating salt bytes
            byte[] saltBytes = GetRandomBytes(saltSize);

            // Appending salt bytes to original bytes
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < originalBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            }

            encryptedBytes = AES_CNG_Encrypt(bytesToBeEncrypted, passwordBytes, _salt_Bytes);

            return Convert.ToBase64String(encryptedBytes);
        }

        public string AES_Decrypt(string encryptedText)
        {
            if (String.IsNullOrEmpty(encryptedText))
                return null;
            if (String.IsNullOrEmpty(_appSettings.EncKey))
                throw new NullReferenceException("key is required");
            var _aES_Password_Bytes = System.Text.Encoding.UTF8.GetBytes(_appSettings.EncKey.Substring(0, 16));
            byte[] passwordBytes = SHA512.Create().ComputeHash(_aES_Password_Bytes);
            byte[] bytesToBeDecrypted = Convert.FromBase64String(encryptedText);


            byte[] decryptedBytes = AES_CNG_Decrypt(bytesToBeDecrypted, passwordBytes, _salt_Bytes);

            // Getting the size of salt
            int saltSize = GetSaltSize(passwordBytes, _salt_Bytes);

            // Removing salt bytes, retrieving original bytes
            byte[] originalBytes = new byte[decryptedBytes.Length - saltSize];
            for (int i = saltSize; i < decryptedBytes.Length; i++)
            {
                originalBytes[i - saltSize] = decryptedBytes[i];
            }

            return Encoding.UTF8.GetString(originalBytes);
        }

        private static byte[] AES_CNG_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, byte[] saltBytes)
        {
            byte[] encryptedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (var _aes = AesCng.Create())
                {
                    _aes.KeySize = 256;
                    _aes.BlockSize = 128;
                    _aes.Mode = CipherMode.ECB;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 10_000);
                    _aes.Key = key.GetBytes(_aes.KeySize / 8);
                    _aes.IV = key.GetBytes(_aes.BlockSize / 8);

                    using (CryptoStream cs = new CryptoStream(ms, _aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }


            //    using (MemoryStream ms = new MemoryStream())
            //{
            //    using (var _encryptor = AesCng.Create())
            //    {
            //        _encryptor.KeySize = 256;
            //        _encryptor.BlockSize = 128;
            //        _encryptor.c

            //        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            //        AES.Key = key.GetBytes(AES.KeySize / 8);
            //        AES.IV = key.GetBytes(AES.BlockSize / 8);

            //        AES.Mode = CipherMode.CBC;

            //        using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
            //        {
            //            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
            //            cs.Close();
            //        }
            //        encryptedBytes = ms.ToArray();
            //    }
            //}

            return encryptedBytes;
        }

        private static byte[] AES_CNG_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] saltBytes)
        {
            byte[] decryptedBytes = null;
            // Set your salt here to meet your flavor:
            //byte[] saltBytes = passwordBytes;
            // Example:
            //saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (var _aes = AesCng.Create())
                {
                    _aes.KeySize = 256;
                    _aes.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 10_000);
                    _aes.Key = key.GetBytes(_aes.KeySize / 8);
                    _aes.IV = key.GetBytes(_aes.BlockSize / 8);

                    _aes.Mode = CipherMode.ECB;

                    using (CryptoStream cs = new CryptoStream(ms, _aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        private static int GetSaltSize(byte[] passwordBytes, byte[] saltBytes)
        {
            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            byte[] _keyBytes = key.GetBytes(2);
            StringBuilder _sb = new StringBuilder();
            for (int i = 0; i < _keyBytes.Length; i++)
            {
                _sb.Append(Convert.ToInt32(_keyBytes[i]).ToString());
            }
            int saltSize = 0;
            string s = _sb.ToString();
            foreach (char c in s)
            {
                int intc = Convert.ToInt32(c.ToString());
                saltSize = saltSize + intc;
            }

            return saltSize;
        }

        private static byte[] GetRandomBytes(int length)
        {
            return RandomNumberGenerator.GetBytes(length);
        }
        #endregion
    }
}
