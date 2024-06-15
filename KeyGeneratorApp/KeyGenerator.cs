using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace KeyGeneratorApp
{

    public class KeyGenerator
    {
        public static void GenerateKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                var privateKey = rsa.ExportRSAPrivateKey();
                var publicKey = rsa.ExportRSAPublicKey();

                File.WriteAllBytes("privateKey.pem", privateKey);
                File.WriteAllBytes("publicKey.pem", publicKey);
            }
        }
    }

}
