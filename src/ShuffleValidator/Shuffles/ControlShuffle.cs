using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShuffleValidator
{
    public class ControlShuffle:IShuffle
    {
        public List<int> Shuffle(IEnumerable<int> cards)
        {
            return cards.ToList();
        }
    }
}
