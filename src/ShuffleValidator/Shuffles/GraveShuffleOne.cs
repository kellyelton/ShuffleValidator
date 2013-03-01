using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
 
namespace ShuffleValidator
{
    public class GraveShuffleOne : IShuffle
    {
        public void Setup(int cardCount, int maxShuffles)
        {
            
        }

        public List<int> Shuffle(IEnumerable<int> cards)
      {
        int[] castedArray = cards.Cast<int>().ToArray();
        int[] arr = ShuffleMethod(castedArray);
        var list = new List<int>(arr);
        return (list);
      }
      
      public int[] ShuffleMethod(int[] array)
        {
          var random = new Random();
            for (int i = array.Length; i > 1; i--)
            {
                // Pick random element to swap.
                int j = random.Next(i); // 0 <= j <= i-1
                // Swap.
                int tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
          return array;
        }
    }
}