using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MiniURL.Application.Common.Interfaces;

namespace MiniURL.Application.Common
{
    public class RNGCrypto : IRNGCrypto
    {
        public void GetBytes(byte[] data)
        {
            using var crypto = new RNGCryptoServiceProvider();
            crypto.GetBytes(data);
        }
    }
}
