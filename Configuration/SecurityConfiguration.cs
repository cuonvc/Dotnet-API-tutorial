using System.Security.Cryptography;
using Demo.DTOs.Request;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Demo.Configuration; 

public class SecurityConfiguration {

    public Student encodePassword(RegisterRequest request) {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);  //bit to byte
        string passwordEncoded = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: request.Password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100,
            numBytesRequested: 256 / 8));
        
        Student student = new Student();
        student.salt = salt;
        student.Password = passwordEncoded;
        return student;
    }

    public string encodePassword(string rawPassword, byte[] salt) {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: rawPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100,
            numBytesRequested: 256 / 8));
    }
}