using System;
using System.Text;

namespace SantaFactory
{
    public static class Varazslas
    {
        public static string Hash(string eredmeny)
        {
            ///SantaFactory2025
            eredmeny += "YrotcaFatnaS2025";

            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(eredmeny))
                );
        }

    }
}