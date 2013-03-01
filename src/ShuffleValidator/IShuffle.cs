using System.Collections.Generic;

namespace ShuffleValidator
{
    public interface IShuffle
    {
        void Setup(int cardCount, int maxShuffles);
        List<int> Shuffle(IEnumerable<int> cards);
    }
}