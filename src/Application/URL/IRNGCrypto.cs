using System;
using System.Collections.Generic;
using System.Text;

namespace MiniURL.Application.URL
{
    public interface IRNGCrypto
    {
        void GetBytes(byte[] data);
    }
}
