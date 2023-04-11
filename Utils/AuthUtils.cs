using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LichTruc.Utils
{
    public class AuthUtils
    {
        public class PasswordHash
        {
            public string? hashed { get; set; }
            public string? salt { get; set; }
        }
        public static PasswordHash hashPassword(string password)
        {            
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {                
                rng.GetBytes(salt);
            }
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: password,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8));
            return new PasswordHash(){ hashed = hashed , salt = Convert.ToBase64String(salt) };
        }

        public static bool checkPasswordHash(string password, string strSalt, string password_hased)
        {
            byte[] salt = Convert.FromBase64String(strSalt);            
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: password,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8));
            return hashed == password_hased;
        }
    }
}
