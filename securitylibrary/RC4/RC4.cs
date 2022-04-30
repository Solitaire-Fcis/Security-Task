using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RC4
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class RC4 : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();
            byte[] cipher = Encoding.ASCII.GetBytes(cipherText);
            byte[] keyByte = Encoding.ASCII.GetBytes(key);
            byte[] plain = new byte[cipherText.Length];
            for (int i = 0; i < cipherText.Length; i++)
            {
                plain[i]=(byte)(cipher[i] ^ keyByte[i]);
            }
            string plaintText = Encoding.ASCII.GetString(plain);
            return plaintText;
        }

        public override  string Encrypt(string plainText, string key)
        {
            throw new NotImplementedException();
        }
    }
}
