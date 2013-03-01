using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ShuffleValidator
{
    public class GraveShuffleTwo : IShuffle
    {
        public List<int> Shuffle(IEnumerable<int> cards)
        {
            int[] castedArray = cards.Cast<int>().ToArray();
            int[] arr = ShuffleMethod(castedArray);
            var list = new List<int>(arr);
            return (list);
        }

        public int[] ShuffleMethod(int[] array)
        {
            var random = new RNGShit();
            for (int i = array.Length; i > 1; i--)
            {
                // Pick random element to swap.
                int j = random.Next(i); // 0 <= j <= i-1
                // Swap.
                int tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
            return array;
        }


        public class RNGShit
        {
            RNGCryptoServiceProvider rnd;
            Byte[] randomBytes = new Byte[4];
            public RNGShit()
            {
                rnd = new RNGCryptoServiceProvider();
            }
            public UInt32 Next()
            {
                rnd.GetBytes(randomBytes);
                return BitConverter.ToUInt32(randomBytes, 0);
            }
            public int Next(int maxValue)
            {
                return this.Next(0, maxValue);
            }
            public Int32 Next(Int32 minValue, Int32 maxValue)
            {
                if (minValue > maxValue)
                    throw new ArgumentOutOfRangeException("minValue");
                if (minValue == maxValue) return minValue;
                Int64 diff = maxValue - minValue;
                while (true)
                {
                    rnd.GetBytes(randomBytes);
                    UInt32 rand = BitConverter.ToUInt32(randomBytes, 0);

                    Int64 max = (1 + (Int64)UInt32.MaxValue);
                    Int64 remainder = max % diff;
                    if (rand < max - remainder)
                    {
                        return (Int32)(minValue + (rand % diff));
                    }
                }
            }
        }
    }
}