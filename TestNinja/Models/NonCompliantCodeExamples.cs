using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace TestNinja.Models
{
    public class NonCompliantCodeExamples
    {
        public void Sample(bool b)
        {
            bool a = false;
            if (a) // Noncompliant
            {
                // never executed
            }

            if (!a || b) // Noncompliant; "!a" is always "true", "b" is never evaluated
            {
                //execute
            }
            else
            {
                // never executed
            }

            var d = "xxx";
            var res = d ?? "value"; // Noncompliant, d is always not null, "value" is never used
        }
        AesManaged aes4 = new AesManaged
        {
            KeySize = 128,
            BlockSize = 128,
            Mode = CipherMode.ECB, // Noncompliant
            Padding = PaddingMode.PKCS7
        };
    }
}
