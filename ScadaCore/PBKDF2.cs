using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ScadaCore
{
    public class PBKDF2
    {
        public static byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[32];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int iterations)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, iterations))
                return rfc2898.GetBytes(32);
        }
    }
    /*
     static void Main(string[] args) { 
    var passwordToHash = "VeryComplexPassword";
    Console.WriteLine("PBKDF2 Hashes\n");
    HashPassword(passwordToHash, 100);
    HashPassword(passwordToHash, 1000);
    HashPassword(passwordToHash, 10000);
    HashPassword(passwordToHash, 50000);
    HashPassword(passwordToHash, 100000);
    HashPassword(passwordToHash, 200000);
    HashPassword(passwordToHash, 500000); 
    } 
    private static void HashPassword(string passwordToHash, 
                                    int numberOfRounds)  
    {  
    var sw = new Stopwatch();
    sw.Start();
    var hashedPassword = PBKDF2.HashPassword(Encoding.UTF8.GetBytes(passwordToHash),
                                            PBKDF2.GenerateSalt(), numberOfRounds);
    sw.Stop();
    Console.WriteLine($"Password to hash: {passwordToHash}");
    Console.WriteLine($"Hashed Password : {Convert.ToBase64String(hashedPassword)}");
    Console.WriteLine($"Iterations: {numberOfRounds}\tElapsed Time: {sw.ElapsedMilliseconds} ms\n"); } */
}