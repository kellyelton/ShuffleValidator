using System.Collections.Generic;

namespace ShuffleValidator
{
    public interface IShuffle
    {
        List<int> Shuffle(IEnumerable<int> cards);
    }
}