namespace ShuffleValidator.Shuffles
{
    using System.Collections.Generic;
    using System.Linq;

    public class ControlShuffleTwo : IShuffle
    {
        public static List<List<int>> AllPossibilites;
        public static int currentIndex = 0;
        public List<int> Shuffle(IEnumerable<int> cards)
        {
            var allCards = cards.ToList();
            if (AllPossibilites == null || AllPossibilites.Count != allCards.Count)
            {
                AllPossibilites = allCards.Permute().Select(x=>x.ToList()).ToList();
                currentIndex = 0;
            }

            if (currentIndex >= allCards.Count) currentIndex = 0;

            var ret = AllPossibilites[currentIndex];

            currentIndex++;
            
            return ret;
        }
    }
}