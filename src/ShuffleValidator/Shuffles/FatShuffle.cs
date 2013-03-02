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
            cards = cards.ToList();
            var num = Random.Next(0, MaxCombinations);
            var e = cards.Permute();

            BigInteger curInt = 0;

            if (num > 0)
            {
                e = e.SkipWhile((x, y) =>
                    {
                        curInt++;
                        return curInt == num;
                    });

            }
            var ret = e.Take(1).First().ToList();

            return ret;
        }
    }
}