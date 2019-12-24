using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ASPNETBlank.Services
{
    public class HashGeneratorService : IHashGeneratorService
    {
        public string GenerateHash(string url)
        {
            byte[] urlBytes = Encoding.ASCII.GetBytes(url);
            AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider();
            aesAlg.GenerateKey();
            aesAlg.GenerateIV();

            byte[] encrypted;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(urlBytes);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            return Convert.ToBase64String(encrypted).Substring(0, 6).Replace('+', RandomChar()).Replace('/', RandomChar()).Replace('?', RandomChar()).ToLower();
        }
        public char RandomChar()
        {
            Random random = new Random();
            const string chars = ")(@=$";
            return chars[random.Next(chars.Length)];
        }
    }
}
