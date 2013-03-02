using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace ShuffleValidator
{
    public static class RNGCryptoServiceProviderExtensionMethods
    {
        public static BigInteger Next(this RNGCryptoServiceProvider random, BigInteger min, BigInteger max)
        {
            if(min > max)
                throw new ArgumentOutOfRangeException("min");
            if (min == max) return min;
            var arrSize = max.ToByteArray().Length;
            var tempArr = new Byte[arrSize];

            // Ok, now we have our random and shit, time to random
            BigInteger ret;
            do
            {
                var diff = max - min;
                random.GetBytes(tempArr);
                var num = new BigInteger(tempArr);
                ret = min + (num%diff);
            } while (ret < min); 
            return ret;
        }
    }
}