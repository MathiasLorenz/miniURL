using Microsoft.Extensions.Configuration;
using MiniURL.Application.Common.Interfaces;
using System;
using System.Text;

namespace MiniURL.Application.Common
{
    // Taken from https://gist.github.com/diegojancic/9f78750f05550fa6039d2f6092e461e5 from discussion on
    // https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IRNGCrypto _crypto;
        private readonly int _tokenLength;

        public TokenGenerator(IRNGCrypto rNGCrypto,
                              IConfiguration configuration)
        {
            _crypto = rNGCrypto;
            
            _tokenLength = GetTokenLength(configuration);
        }

        private int GetTokenLength(IConfiguration configuration)
        {
            int length = configuration.GetValue<int>("ShortURLStringLength");

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_tokenLength));
            }

            return length;
        }

        public string GetUniqueKey()
        {
            // The chars array "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890.-" has length 64 and
            // therefore divides the size of a byte evenly (256 % 64 = 0).
            // If another array is used that does not divide 64 this has to be taken care of.
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_-".AsSpan();
            int charsLength = chars.Length; // Has to be exactly 64 to divide a byte.
            var data = new byte[_tokenLength];

            _crypto.GetBytes(data);

            var builder = new StringBuilder(_tokenLength);

            foreach (var b in data)
            {
                builder.Append(chars[b % (charsLength)]);
            }

            return builder.ToString();
        }
    }
}
