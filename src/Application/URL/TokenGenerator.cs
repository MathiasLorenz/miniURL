using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MiniURL.Application.URL
{
    // Taken from https://gist.github.com/diegojancic/9f78750f05550fa6039d2f6092e461e5 from discussion on
    // https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
    public static class TokenGenerator
    {
        public static string GetUniqueKey(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(GetUniqueKey));
            }

            // The chars array "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890.-" has length 64 and
            // therefore divides the size of a byte evenly (256 % 64 = 0).
            // If another array is used that does not divide 64 this has to be taken care of.
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890.-".AsSpan();
            int charsLength = chars.Length; // Has to be exactly 64 to divide a byte.
            var data = new byte[length];

            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }

            var builder = new StringBuilder(length);

            foreach(var b in data)
            {
                builder.Append(chars[b % (charsLength)]);
            }

            return builder.ToString();
        }
    }
}
