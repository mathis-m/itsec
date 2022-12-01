using System;
using System.IO;

namespace EncryptDecrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            var vigenereCipher = new VigenereCipher();

            Console.WriteLine("Choose method:\n\n[0] Encrypt\n[1] Decrypt");
            var line = Console.ReadLine();
            var text = GetTextInput();
            Console.WriteLine("Enter Key:");
            var key = Console.ReadLine();

            if (line != null && line.StartsWith("0"))
            {
                Console.WriteLine(vigenereCipher.Encrypt(text, key));
            } else if (line != null && line.StartsWith("1"))
            {
                Console.WriteLine(vigenereCipher.Decrypt(text, key));
            }
        }

        static string GetTextInput()
        {
            Console.WriteLine("Enter Path for text:");
            var path = Console.ReadLine();

            return File.ReadAllText(path ?? throw new InvalidOperationException("path can not be null"));
        }
    }
}
