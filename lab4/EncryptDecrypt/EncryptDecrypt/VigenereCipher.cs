using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptDecrypt
{
    public class VigenereCipher
    {
        private static int Mod(int a, int b)
        {
            return (a % b + b) % b;
        }

        private string EncryptDecrypt(string text, string key, bool encrypt)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("key cannot be empty");

            var returnText = new StringBuilder();
            var nonAlphaCharCount = 0;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var keyIndex = (i - nonAlphaCharCount) % key.Length;
                var keyChar = key[keyIndex];
                if (char.IsLetter(c))
                {
                    var isCurrentCharUpper = char.IsUpper(c);
                    var offset = isCurrentCharUpper ? 'A' : 'a';

                    var k = (isCurrentCharUpper ? char.ToUpper(keyChar) : char.ToLower(keyChar)) - offset;
                    k = encrypt ? k : -k;
                    var newChar = (char)((Mod(((c + k) - offset), 26)) + offset);
                    returnText.Append(newChar);
                }
                else
                {
                    returnText.Append(c);
                    ++nonAlphaCharCount;
                }
            }

            return returnText.ToString();
        }

        public string Encrypt(string plainText, string key)
        {
            return EncryptDecrypt(plainText, key, true);
        }

        public string Decrypt(string encrypted, string key)
        {
            return EncryptDecrypt(encrypted, key, false);
        }
    }
}
