using System;

namespace CryptoFoi.Core.Exceptions
{
    public class BadUninitializationDataException: Exception
    {
        public BadUninitializationDataException()
        :base("Wrong input parametars."){ }

        public BadUninitializationDataException(string msg)
        :base(msg){ }
    }
}