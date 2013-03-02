using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using ShuffleValidator.Shuffles;

namespace ShuffleValidator
{
    public static class RNGCryptoServiceProviderExtensionMethods
    {
        public static BigInteger Next(this RNGCryptoServiceProvider random, BigInteger max)
        {
            var arrSize = max.ToByteArray().Length.NextPowerOfTwo();
            var tempArr = new Byte[arrSize];
            random.GetBytes(tempArr);
            var next = new BigInteger(tempArr) & 0x7fffffff;
            return next % max;
        }

        public static BigInteger Next(this RNGCryptoServiceProvider random, BigInteger min, BigInteger max)
        {
            if(min > max)
                throw new ArgumentOutOfRangeException("min");
            if (min == max) return min;

            var range = max - min;
            return min + random.Next(range);
        }
    }
}