using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoFoi.Core.CryptoCoding
{
    public interface ICrypto
    {
        void Encrypt(string fileName);
        string Decrypt(string fileName);
        void Sign();
        bool CheckSignature();
    }
}