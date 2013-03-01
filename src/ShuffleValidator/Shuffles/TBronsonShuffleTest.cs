using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ShuffleValidator
{
    public class TBronsonShuffleTest : IShuffle
    {
        public List<int> cards;
        private static RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();
        // Quick sort pile to shuffle. Return false if 2 cards were assigned equal values.
        // Standard quick sort algo. Repeatedly repartition into lower and larger partitions
        // around a pivot point that starts in the middle.
        internal bool SortShuffle(uint[] a, int left, int right, Exception[] cis, bool local)
        {
            int i = left;
            int j = right;
            // pivot point that forms the 2 partitions
            uint pivot = a[(left + right) / 2];
            uint w = 0;
            while (i <= j)
            {
                // find first entry in left partition not smaller than pivot
                while (a[i] < pivot)
                {
                    i++;
                }
                // find last entry in right partition not larger than pivot
                while (pivot < a[j])
                {
                    j--;
                }
                // was there a larger entry left of pivot or smaller entry right of pivot?
                if (i <= j)
                {
                    // if 2 cards were assigned the same value (extremely rare chance)
                    // we will abort and start over to ensure a proper unbiased shuffle.
                    if (a[i] == w || a[j] == w)
                        return false;
                    // for DoShuffle()
                    //if (cis != null)
                    //{
                    //    CardIdentity ci = cis[i];
                    //    cis[i] = cis[j];
                    //    cis[j] = ci;
                    //}
                    // for ShuffleAlone()
                    if (local)
                    {
                        var temp = cards[i];
                        cards[i] = cards[j];
                        cards[j] = temp;
                    }

                    // swap them around the pivot
                    w = a[i];
                    a[i++] = a[j];
                    a[j--] = w;
                }
            }
            // continue sort on the left partition
            if (left < j)
            {
                if (!SortShuffle(a, left, j, cis, local))
                    return false;
            }
            // continue sort on the right partition
            if (i < right)
            {
                if (!SortShuffle(a, i, right, cis, local))
                    return false;
            }
            return true;
        }

        public List<int> Shuffle(IEnumerable<int> cards)
        {
            var cardList = cards.ToList();
            this.cards = cardList;
            var rndbytes = new Byte[4];
            var cardRnds = new uint[cardList.Count];
            do
            {
                for (int i = 0; i < cardList.Count; i++)
                {
                    rnd.GetBytes(rndbytes);
                    cardRnds[i] = BitConverter.ToUInt32(rndbytes, 0);
                }
            } while (!SortShuffle(cardRnds, 0, cardList.Count - 1,null, true));
            return this.cards;
        }
    }
}
