using System.Collections.Generic;
using System.Linq;
using Shuffle.Net;

namespace ShuffleValidator
{
    public class QuantumShuffleTest : IShuffle
    {
        public void Setup(int cardCount, int maxShuffles)
        {
            
        }

        public List<int> Shuffle(IEnumerable<int> cards)
        {
            var list = cards.ToList();
            var qlist = new QuantumList<int>();
            foreach(var l in list)
                qlist.Add(l);

            while (qlist.UnknownItems.Any())
            {
                foreach (var i in qlist)
                {
                    var b = i;
                }
            }

            return qlist.ToList();
        }
    }
}