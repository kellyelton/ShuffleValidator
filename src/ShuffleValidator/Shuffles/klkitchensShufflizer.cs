using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShuffleValidator.Shuffles
{
	public class klkitchensShufflizer:IShuffle
	{
		private readonly Random _randgen;

		public klkitchensShufflizer()
		{
			_randgen = new Random();
		}

		#region IShuffle Members

		public void Setup(int cardCount, int maxShuffles)
		{
		}

		public List<int> Shuffle(IEnumerable<int> cards)
		{
			var newcards = (from id in cards
			                select new {cardId = id, randOrder = _randgen.Next(1, 100000)}).ToList();

			return newcards.OrderBy(card => card.randOrder).Select(card => card.cardId).ToList();

		}

		#endregion

		public string CreateCheckCode(int iLength)
		{
			string sCode = "";

			const string sVALUES = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

			for (int iLoop = 1; iLoop <= iLength; iLoop++)
			{
				sCode += sVALUES.Substring(_randgen.Next(1, sVALUES.Length), 1);
			}

			return sCode;
		}

		public int CreateCheckNumber(int iLength)
		{
			string sCode = "";

			const string sVALUES = "0123456789";

			for (int iLoop = 1; iLoop <= iLength; iLoop++)
			{
				sCode += sVALUES.Substring(_randgen.Next(1, sVALUES.Length), 1);
			}

			return Int32.Parse(sCode);
		}


	}
}
