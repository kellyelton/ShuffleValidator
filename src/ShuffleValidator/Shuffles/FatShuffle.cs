using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace ShuffleValidator.Shuffles
{
    public class FatShuffle : IShuffle
    {
        public static RNGCryptoServiceProvider Random = new RNGCryptoServiceProvider();
        public static Byte[] RandomBuffer = new byte[8];
        public int CardCount { get; set; }
        public int MaxShuffles { get; set; }
        public BigInteger MaxCombinations { get; set; }

        public void Setup(int cardCount, int maxShuffles)
        {
            CardCount = cardCount;
            MaxShuffles = maxShuffles;
            MaxCombinations = CardCount.Factorial();
        }

        public List<int> Shuffle(IEnumerable<int> cards)
        {
            var num = Random.Next(0,CardCount);
            var e = cards.Permute();
            var curSkip = 1;
            for (BigInteger i = 0; i < num; i++)
            {
                if (curSkip == int.MaxValue)
                {
                    e = e.Skip(curSkip);
                    curSkip = 1;
                }
                else if (i == num - 1)
                {
                    e = e.Skip(curSkip);
                }
                curSkip++;
            }
            return e.Take(1).First().ToList();
        }
    }
}