using System;
using System.Collections.Generic;

namespace ShuffleValidator
{
    using System.Linq;

    public class ShuffleValidator
    {
        public ShuffleValidator(Type t)
        {
            
        }


    }
    public class AnalizeIndex
    {
        public Dictionary<int, Dictionary<int, int>> Counts;
        public int RunCount;
        public int CardCount;

        public AnalizeIndex(List<List<int>> cards,int cardCount, int runCount)
        {
            CardCount = cardCount;
            RunCount = runCount;
            Counts = new Dictionary<int, Dictionary<int, int>>();

            for (var i = 0; i < cardCount;i++ )
            {
                Counts[i] = new Dictionary<int, int>();
                for (var c = 1; c <= cardCount; c++)
                {
                    Counts[i][c] = 0;
                }
            }

            foreach (var deck in cards)
            {
                for (var i = 0; i < deck.Count; i++)
                {
                    if(!Counts[i].ContainsKey(deck[i]))
                        Counts[i].Add(deck[i],0);
                    Counts[i][deck[i]] += 1;
                }
            }
        }

        public double GetRating()
        {
            var resultList = new List<double>();
            foreach (var c in Counts)
            {
                var res = 0d;
                var median = RunCount / (double)CardCount;
                foreach (var index in c.Value)
                {
                    var diff = (index.Value > median) ? index.Value - median : median - index.Value;
                    res = Math.Abs(diff);
                    res += (index.Value > median) ? median : -median;
                }
                resultList.Add(res);
            }
            var ret = (resultList.Aggregate((x,y)=>x+y) / Counts.Count) * Counts.Count;
            ret = ret / RunCount;
            ret = (ret + 1)/2;//because the best shuffle is -1 and the worse is 1, so +1 to get between 0 and 1
            ret = ret * 100;//Multiply by 100 to give us a 0-100
            ret = 100 - ret;//This inverts our score, so the higher number is better.
            return ret;
        }
    }
}