﻿using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using todo_backend.Services.Interface;

namespace todo_backend.Services
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;
        private readonly HashAlgorithmName _algorithm= HashAlgorithmName.SHA256;

        public string HashedPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password , salt , Iterations, _algorithm , HashSize);
            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public bool VerifyPassowrd(string password, string passwordHash)
        {
            string[] parts = passwordHash.Split('-'); //spliting the password to get the salt 
            byte[] hash= Convert.FromHexString(parts[0]); // Hashed password
            byte[] salt= Convert.FromHexString(parts[1]); // salt

            // Hashing the input password 
            byte[] inputhash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _algorithm, HashSize);

            //return hash.SequenceEqual(inputhash);
            return CryptographicOperations.FixedTimeEquals(hash, inputhash);
        }

        


    }
}
