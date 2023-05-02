using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Demo.Configuration; 

public class SecurityConfiguration {

    public string[] encodePassword(string rawPassword) {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: rawPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        return new[] { Convert.ToBase64String(salt), hashed };
    }
}