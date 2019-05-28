using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using FileUploadApp.Interfaces;

namespace FileUploadApp.Services.Accounts
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        private const int Pbkdf2IterCount = 1000; // default for Rfc2898DeriveBytes
        private const int Pbkdf2SubKeyLength = 256 / 8; // 256 bits
        private const int SaltSize = 128 / 8; // 128 bits

        /* =======================
         * HASHED PASSWORD FORMATS
         * =======================
         * 
         * Version 0:
         * PBKDF2 with HMAC-SHA1, 128-bit salt, 256-bit subKey, 1000 iterations.
         * (See also: SDL crypto guidelines v5.1, Part III)
         * Format: { 0x00, salt, subKey }
         */

        public string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            // Produce a version 0 (see comment above) password hash.
            byte[] salt;
            byte[] subKey;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Pbkdf2IterCount))
            {
                salt = deriveBytes.Salt;
                subKey = deriveBytes.GetBytes(Pbkdf2SubKeyLength);
            }

            var outputBytes = new byte[1 + SaltSize + Pbkdf2SubKeyLength];

            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subKey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubKeyLength);

            return Convert.ToBase64String(outputBytes);
        }

        // hashedPassword must be of the format of HashWithPassword (salt + Hash(salt+input)
        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

            // Verify a version 0 (see comment above) password hash.

            if (hashedPasswordBytes.Length != (1 + SaltSize + Pbkdf2SubKeyLength) || hashedPasswordBytes[0] != 0x00)
            {
                // Wrong length or version header.
                return false;
            }

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            var storedSubKey = new byte[Pbkdf2SubKeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubKey, 0, Pbkdf2SubKeyLength);

            byte[] generatedSubKey;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Pbkdf2IterCount))
            {
                generatedSubKey = deriveBytes.GetBytes(Pbkdf2SubKeyLength);
            }

            return ByteArraysEqual(storedSubKey, generatedSubKey);
        }

        // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Count != b.Count)
            {
                return false;
            }

            var areSame = true;

            for (var i = 0; i < a.Count; i++)
            {
                areSame &= (a[i] == b[i]);
            }

            return areSame;
        }
    }
}