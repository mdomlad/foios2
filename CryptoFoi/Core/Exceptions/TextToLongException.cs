using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoFoi.Core.Exceptions
{
    public class TextToLongException : Exception
    {
        public TextToLongException()
        :base("Max character limit exceded."){ }

        public TextToLongException(string msg)
        :base(msg){ }
    }
}