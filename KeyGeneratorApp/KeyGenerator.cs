using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

using System;
using System.IO;

namespace KeyGeneratorApp
{
    public class KeyGenerator
    {
        public static void GenerateKeys()
        {
            using (RSA rsa = RSA.Create(2048)) // Koristimo RSA.Create umesto RSACryptoServiceProvider
            {
                // Izvoz privatnog ključa u formatu PKCS#1
                byte[] privateKeyBytes = rsa.ExportRSAPrivateKey();
                File.WriteAllBytes("privateKey.pem", privateKeyBytes);

                // Izvoz javnog ključa u formatu X.509 SubjectPublicKeyInfo
                byte[] publicKeyBytes = rsa.ExportRSAPublicKey();
                File.WriteAllBytes("publicKey.pem", publicKeyBytes);
            }
        }
    }
}

