using System;
using System.Collections.Generic;
using System.Linq;

namespace ShuffleValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            var asses = AppDomain.CurrentDomain.GetAssemblies();
            var shuffleTypeList = new List<Type>();
            var ignoreList = new List<Type>();
            foreach (var ass in asses)
            {
                shuffleTypeList.AddRange(ass.GetExportedTypes().Where(x => x.GetInterfaces().Any(y => y == typeof (IShuffle))));
            }

            var resultList = new Dictionary<Type,double>();

            //ignoreList.Add(typeof(ControlShuffle));
            //ignoreList.Add(typeof(ControlShuffleTwo));

            foreach (var stype in shuffleTypeList.Where(x=>!ignoreList.Contains(x)))
            {
                Console.WriteLine("##Testing shuffle type {0} ##", stype.Name);

                const int runCount = 1000;
                const int maxDeckSize = 200;


                var ratingList = new List<double>();

                for (var cardCount = 2; cardCount < maxDeckSize; cardCount++)
                {
                    var top = Console.CursorTop;
                    DrawProgressBar(cardCount, maxDeckSize - 1, '#', "");
                    Console.WriteLine();
                    var results = RunTest(cardCount, stype, runCount);
                    ratingList.Add(AnalizeResults(results, cardCount, runCount));
                    Console.CursorTop = top;
                }

                // Average rating
                var rating = (ratingList.Aggregate((x, y) => x + y)/ratingList.Count);
                var finalRating = rating;//100-((rating > 50 ? rating - 50 : 50 - rating) * 2);

                resultList.Add(stype, finalRating);

                Console.CursorTop += 3;
                //Console.WriteLine("Rating: {0}/100", finalRating);
                Console.WriteLine("");

            }

            Console.WriteLine();

            int number = 1;
            foreach (var rating in resultList.OrderByDescending(x => x.Value))
            {
                Console.WriteLine("#{0} - {1}\n\tScore: {2}/100", number, rating.Key.Name, rating.Value);
                number++;
            }

            Console.WriteLine("Any key to exit.");
            Console.ReadLine();
        }

        static double AnalizeResults(List<List<int>> cards, int cardCount, int runCount)
        {

            var anal = new AnalizeIndex(cards, cardCount, runCount);
            return anal.GetRating();
        }

        static List<List<int>> RunTest(int cardCount, Type shuffleType, int runCount)
        {
            //runCount = cardCount.Factorial();
            var results = new List<List<int>>();
            var locker = new object();

            var repeatcount = new List<int>();
            for (var i = 0; i < runCount;i++ )
                repeatcount.Add(i);

            var curInt = 0;
            var shuffle = (IShuffle)Activator.CreateInstance(shuffleType);
            shuffle.Setup(cardCount,runCount);
            do
            {
                var cards = CreateCardList(cardCount);
                var result = shuffle.Shuffle(cards);
                results.Add(result);
                DrawProgressBar(curInt, runCount - 1, '#', "");
                curInt++;
            }
            while (curInt < runCount);

            return results;
        }

        static List<int> CreateCardList(int count)
        {
            var cards = new List<int>();
            for (var c = 1; c <= count; c++)
            {
                cards.Add(c);
            }
            return cards;
        }

        private static void DrawProgressBar(int complete, int maxVal, char progressCharacter, string message)
        {
            int barSize = Console.BufferWidth - 14;
            Console.CursorVisible = false;
            int top = Console.CursorTop;
            Console.WriteLine(message);
            int left = Console.CursorLeft;
            decimal perc = (decimal)complete / (decimal)maxVal;
            int chars = (int)Math.Floor(perc / ((decimal)1 / (decimal)barSize));
            string p1 = String.Empty, p2 = String.Empty;

            for (int i = 0; i < chars; i++) p1 += progressCharacter;
            for (int i = 0; i < barSize - chars; i++) p2 += progressCharacter;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(p1);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(p2);
            
            Console.ResetColor();
            Console.Write(" {0}%", (perc * 100).ToString("N2"));
            Console.CursorLeft = left;
            Console.CursorTop = top;
        } 
    }
}
