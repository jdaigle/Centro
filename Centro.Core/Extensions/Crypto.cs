using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Centro.Extensions
{
    public static class Crypto
    {
        public enum HashAlgorithm
        {
            MD5,
            SHA1,
            //SHA256,
            //SHA384,
            //SHA512,
        }

        public enum SymmetricAlgorithm
        {
            Aes,
            //DES,
            RC2,
            TripleDES,
        }

        /// <summary>
        /// Hashes the specified string using the specified algorithm.
        /// </summary>
        /// <param name="plaintext">The plaintext.</param>
        /// <param name="algorithm">The hashing algorithm to use.</param>
        /// <returns>The base64 encoded hash string.</returns>
        public static string Hash(this string plaintext, HashAlgorithm algorithm)
        {
            if (string.IsNullOrEmpty(plaintext))
                throw new ArgumentNullException("plaintext", "Cannot hash an empty string.");
            System.Security.Cryptography.HashAlgorithm hashAlgorithm;
            switch (algorithm)
            {                
                case HashAlgorithm.MD5:
                    hashAlgorithm = new MD5CryptoServiceProvider();
                    break;                
                case HashAlgorithm.SHA1:
                default:
                    hashAlgorithm = new SHA1CryptoServiceProvider();
                    break;
                //case HashAlgorithmEnum.SHA256:
                //    hashAlgorithm = new SHA256CryptoServiceProvider();
                //    break;
                //case HashAlgorithmEnum.SHA384:
                //    hashAlgorithm = new SHA384CryptoServiceProvider();
                //    break;                
                //case HashAlgorithmEnum.SHA512:
                //    hashAlgorithm = new SHA512CryptoServiceProvider();
                //    break;
            }
            var unhashedBuffer = UTF8Encoding.Default.GetBytes(plaintext);
            var hashedBuffer = hashAlgorithm.ComputeHash(unhashedBuffer);
            return Convert.ToBase64String(hashedBuffer);
        }

        /// <summary>
        /// Encryptes the text using the specified symmetric algorithm and the provided key.
        /// </summary>
        /// <param name="plaintext">The plaintext.</param>
        /// <param name="key">The secret key for the symmetric algorithm.</param>
        /// <param name="algorithm">The symmetric algorithm to use.</param>
        /// <returns>The base64 encoded cipher text string.</returns>
        public static string SymmetricEncrypt(this string plaintext, string key, SymmetricAlgorithm algorithm)
        {
            if (string.IsNullOrEmpty(plaintext))
                throw new ArgumentNullException("plaintext", "Cannot encrypt an empty string.");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "Cannot encrypt with an empty key.");

            byte[] keyBuffer = Convert.FromBase64String(key.Hash(HashAlgorithm.MD5));
            byte[] plainTextBuffer = UTF8Encoding.UTF8.GetBytes(plaintext);

            System.Security.Cryptography.SymmetricAlgorithm symmetricAlgorithm;            
            switch(algorithm)
            {                
                //case SymmetricAlgorithmEnum.DES:
                //    symmetricAlgorithm = new DESCryptoServiceProvider();
                //    break;
                case SymmetricAlgorithm.RC2:
                    symmetricAlgorithm = new RC2CryptoServiceProvider();
                    break;
                case SymmetricAlgorithm.TripleDES:
                    symmetricAlgorithm = new TripleDESCryptoServiceProvider();
                    break;
                case SymmetricAlgorithm.Aes:
                default:
                    symmetricAlgorithm = new AesCryptoServiceProvider();
                    break;
            }

            symmetricAlgorithm.Key = keyBuffer;
            symmetricAlgorithm.Mode = CipherMode.ECB;
            symmetricAlgorithm.Padding = PaddingMode.PKCS7;

            var encryptor = symmetricAlgorithm.CreateEncryptor();
            byte[] cipherBuffer = encryptor.TransformFinalBlock(plainTextBuffer, 0, plainTextBuffer.Length);
            symmetricAlgorithm.Clear();

            return Convert.ToBase64String(cipherBuffer);
        }

        /// <summary>
        /// Decryptes the text using the specified symmetric algorithm and the provided key.
        /// </summary>
        /// <param name="cipherText">The base64 encoded cipher text string.</param>
        /// <param name="key">The secret key for the symmetric algorithm.</param>
        /// <param name="algorithm">The symmetric algorithm to use.</param>
        /// <returns>The plain text.</returns>
        public static string SymmetricDecrypt(this string cipherText, string key, SymmetricAlgorithm algorithm)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText", "Cannot decrypt an empty cipher text.");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "Cannot decrypt with an empty key.");

            byte[] keyBuffer = Convert.FromBase64String(key.Hash(HashAlgorithm.MD5));
            byte[] cipherTextBuffer = Convert.FromBase64String(cipherText);

            System.Security.Cryptography.SymmetricAlgorithm symmetricAlgorithm;
            switch (algorithm)
            {
               // case SymmetricAlgorithmEnum.DES:
               //     symmetricAlgorithm = new DESCryptoServiceProvider();
               //     break;
                case SymmetricAlgorithm.RC2:
                    symmetricAlgorithm = new RC2CryptoServiceProvider();
                    break;
                case SymmetricAlgorithm.TripleDES:
                    symmetricAlgorithm = new TripleDESCryptoServiceProvider();
                    break;
                case SymmetricAlgorithm.Aes:
                default:
                    symmetricAlgorithm = new AesCryptoServiceProvider();
                    break;
            }

            symmetricAlgorithm.Key = keyBuffer;
            symmetricAlgorithm.Mode = CipherMode.ECB;
            symmetricAlgorithm.Padding = PaddingMode.PKCS7;

            var decryptor = symmetricAlgorithm.CreateDecryptor();
            byte[] plainTextBuffer = decryptor.TransformFinalBlock(cipherTextBuffer, 0, cipherTextBuffer.Length);
            symmetricAlgorithm.Clear();

            return UTF8Encoding.Default.GetString(plainTextBuffer);
        }

        /// <summary>
        /// Computes a hash value of the string, and signs the resulting hash value with the RSA private key supplied.
        /// </summary>
        /// <param name="plaintext">The plaintext.</param>
        /// <param name="rsaKey">The RSA private key.</param>
        /// <returns>The base64 encoded signed hash string.</returns>
        public static string RSASign(this string plaintext, RSAParameters rsaKey)
        {
            if (string.IsNullOrEmpty(plaintext))
                throw new ArgumentNullException("plaintext", "Cannot sign an empty string.");

            byte[] plainTextBuffer = UTF8Encoding.UTF8.GetBytes(plaintext);
            var rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.ImportParameters(rsaKey);
            byte[] signedHashBuffer = rsaProvider.SignData(plainTextBuffer, new SHA1CryptoServiceProvider());
            rsaProvider.Clear();
            return Convert.ToBase64String(signedHashBuffer);
        }

        /// <summary>
        /// Verifies the string with the base64 encoded signed hash value provided, and the RSA public key supplied.
        /// </summary>
        /// <param name="plaintext">The plaintext.</param>
        /// <param name="signedHash">The signed hash.</param>
        /// <param name="rsaKey">The RSA public key.</param>
        /// <returns>True if the signature is verified aga</returns>
        public static bool RSAVerifySigned(this string plaintext, string signedHash, RSAParameters rsaKey)
        {
            if (string.IsNullOrEmpty(plaintext))
                throw new ArgumentNullException("plaintext", "Cannot verify an empty string.");
            if (string.IsNullOrEmpty(signedHash))
                throw new ArgumentNullException("signedHash", "Cannot verify against an empty signed hash.");            

            byte[] plainTextBuffer = UTF8Encoding.UTF8.GetBytes(plaintext);
            byte[] signedHashBuffer = Convert.FromBase64String(signedHash);
            var rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.ImportParameters(rsaKey);
            var verified = rsaProvider.VerifyData(plainTextBuffer, new SHA1CryptoServiceProvider(), signedHashBuffer);
            rsaProvider.Clear();
            return verified;
        }        
    }
}
