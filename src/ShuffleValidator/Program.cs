using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShuffleValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            var asses = AppDomain.CurrentDomain.GetAssemblies();
            var shuffleTypeList = new List<Type>();
            foreach (var ass in asses)
            {
                shuffleTypeList.AddRange(ass.GetExportedTypes().Where(x => x.GetInterfaces().Any(y => y == typeof (IShuffle))));
            }

            var resultList = new Dictionary<Type,double>();

            foreach (var stype in shuffleTypeList)
            {
                Console.WriteLine("##Testing shuffle type {0} ##", stype.Name);

                const int runCount = 1000;
                const int maxDeckSize = 200;


                var ratingList = new List<double>();
                for (var cardCount = 2; cardCount < maxDeckSize; cardCount++)
                {
                    var top = Console.CursorTop;
                    DrawProgressBar(cardCount, maxDeckSize - 1, Console.BufferWidth - 10, '#', "");
                    Console.WriteLine();
                    var results = RunTest(cardCount, stype,runCount);
                    ratingList.Add(AnalizeResults(results, cardCount,runCount));
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

            //var results = new double[cardCount];

            //foreach (var card in cards)
            //{
            //    for (var i = 0; i < card.Count; i++)
            //    {
            //        results[i] += card[i];
            //    }
            //}

            //for (var i = 0; i < cardCount; i++)
            //{
            //    results[i] = (results[i] / runCount) / cardCount;
            //}

            //var rating = ((results.Aggregate((x,y)=>x+y) / results.Count()) * 100);
            //return rating;
        }

        static List<List<int>> RunTest(int cardCount, Type shuffleType, int runCount)
        {
            var results = new List<List<int>>();
            for (var repeatCount = 0; repeatCount < runCount; repeatCount++)
            {
                var cards = CreateCardList(cardCount);
                var shuffle = (IShuffle)Activator.CreateInstance(shuffleType);
                var result = shuffle.Shuffle(cards);
                results.Add(result);
                DrawProgressBar(repeatCount, runCount - 1, Console.BufferWidth - 10, '#', "");
            }
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

        private static void DrawProgressBar(int complete, int maxVal, int barSize, char progressCharacter, string message)
        {
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
