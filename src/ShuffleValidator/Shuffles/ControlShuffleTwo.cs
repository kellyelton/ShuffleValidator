namespace ShuffleValidator.Shuffles
{
    using System.Collections.Generic;
    using System.Linq;

    public class ControlShuffleTwo : IShuffle
    {
        public static List<List<int>> AllPossibilites;
        public static int CurrentIndex = 0;
        public int CardCount { get; set; }
        public int MaxShuffles { get; set; }
        public void Setup(int cardCount, int maxShuffles)
        {
            CardCount = cardCount;
            MaxShuffles = maxShuffles;
            AllPossibilites = null;
            CurrentIndex = 0;
        }
        public List<int> Shuffle(IEnumerable<int> cards)
        {
            if (CurrentIndex >= CardCount) CurrentIndex = 0;

            var ret = cards.Permute().Select(x => x.Take(CardCount).ToList()).Skip(CurrentIndex).First();

            CurrentIndex++;
            
            return ret;
        }
    }
}