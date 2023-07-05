using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Utility.ExtensionMethod
{
   public class RandomStringGenerator
    {
        public static string GenerateRandomString(int no_of_char, string typeOfAlphabets)
        {
            string randomStr = string.Empty;
            var chars = "";

            if (typeOfAlphabets == "Mixed")
            {
                chars = "ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijklmnpqrst123456789";
            }
            else if (typeOfAlphabets == "Capital")
            {
                chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            }
            else if (typeOfAlphabets == "Running")
            {
                chars = "abcdefghijklmnpqrst123456789";
            }
            else if (typeOfAlphabets == "Numeric")
            {
                chars = "123456789";
            }

            var stringChars = new char[no_of_char];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            randomStr = new String(stringChars);
            return randomStr;

        }
    }
}
