using System;
using System.Numerics;

namespace ShuffleValidator
{
    using System.Collections.Generic;
    using System.Linq;

    public static class MathExtensionMethods
    {
        public static int NextPowerOfTwo(this int input)
        {
            return 2.Powers().First(x => x > input);
        }

        public static IEnumerable<int> Powers(this int baseNumber)
        {
            var result = 1;
            do
            {
                result = result * baseNumber;
                if(result > int.MaxValue)
                    yield break;
                yield return result;
            }
            while (result < int.MaxValue);
        }

        public static BigInteger Factorial(this int input)
        {
            var res = new BigInteger(1);
            for (var i = 1; i <= input; i++)
            {
                res *= i;
            }
            return res;
        }
        public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                yield break;
            }

            var list = sequence.ToList();

            if (list.Any())
            {
                var startingElementIndex = 0;

                foreach (var startingElement in list)
                {
                    var remainingItems = list.AllExcept(startingElementIndex);

                    foreach (var permutationOfRemainder in remainingItems.Permute())
                    {
                        yield return startingElement.Concat(permutationOfRemainder);
                    }

                    startingElementIndex++;
                }
            }
            else
            {
                yield return Enumerable.Empty<T>();
            }
        }

        private static IEnumerable<T> Concat<T>(this T firstElement, IEnumerable<T> secondList)
        {
            yield return firstElement;
            foreach (var item in secondList)
            {
                yield return item;
            }
        }

        private static IEnumerable<T> AllExcept<T>(this IEnumerable<T> sequence, int indexToSkip)
        {
            var index = 0;

            foreach (var item in sequence)
            {
                if (index != indexToSkip)
                {
                    yield return item;
                }

                index++;
            }
        }
    }
}
