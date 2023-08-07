using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Basic_ATM.Models
{
    internal class PasswordsHashingWithSalt
    {

        int keySize = 20;
        int iterations = 2000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public string CreatePasswordForUser(string password, out byte[] salt)//PBKDF2
        {

            salt = RandomNumberGenerator.GetBytes(keySize);  //prideda random dalyku prie passwordo
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        public bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }



    }
}
