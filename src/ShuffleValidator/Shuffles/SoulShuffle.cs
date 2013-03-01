using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ShuffleValidator
{
    public class SoulShuffle: IShuffle
    {
        public void Setup(int cardCount, int maxShuffles)
        {
            
        }

        public List<int> Shuffle(IEnumerable<int> cards)
        {
            int[] castedArray = cards.Cast<int>().ToArray();
            int[] arr = ShuffleMethod(castedArray);
            var list = new List<int>(arr);
            return (list);
        }

        public int[] ShuffleMethod(int[] array)
        {
            var random = new CryptoRNG();
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


        class CryptoRNG
        {
            RNGCryptoServiceProvider rnd;
            Byte[] randomBytes = new Byte[4];
            public CryptoRNG()
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
            public int Next(int minValue, int maxValue)
            {
                if (minValue > maxValue) throw new ArgumentOutOfRangeException();
                if (minValue == maxValue) return minValue;
                int diff = maxValue - minValue;
                double percent = this.NextDouble();
                return (int)(percent * diff + minValue);
            }
            public double NextDouble()
            {
                return this.Next() / ((double)(UInt32.MaxValue) + 1);
            }
        }
    }
}