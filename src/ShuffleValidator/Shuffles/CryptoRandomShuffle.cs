using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ShuffleValidator
{
    public class CryptoRandomShuffle : IShuffle
    {
        public CryptoRandom cryptoRng;
        public void Setup(int cardCount, int maxShuffles)
        {
            cryptoRng = new CryptoRandom();
        }

        public List<int> Shuffle(IEnumerable<int> cards)
        {
            int[] castedArray = cards.ToArray();
            int[] arr = CryptoShuffle(castedArray);
            var list = new List<int>(arr);
            return (list);
        }

        public int[] CryptoShuffle(int[] array)
        {
            var random = cryptoRng;
            for (int i = array.Length - 1; i > 1; i--)
            {
                // Pick random element to swap.
                int j = random.Next(i + 1); // 0 <= j <= i
                // Swap.
                int tmp = array[j];
                array[j] = array[i];
                array[i] = tmp;
            }
            return array;
        }

	public class CryptoRandom : Random 
	{ 
		private RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider(); 
		private byte[] _uint32Buffer = new byte[4]; 
		public CryptoRandom() { } 
		public CryptoRandom(Int32 ignoredSeed) { } 
		public override Int32 Next() { 
			_rng.GetBytes(_uint32Buffer); 
			return BitConverter.ToInt32(_uint32Buffer, 0) & 0x7FFFFFFF;
		} 
		public override Int32 Next(Int32 maxValue) { 
			if (maxValue < 0) throw new ArgumentOutOfRangeException("maxValue"); 
			return Next(0, maxValue); 
		} 
		public override Int32 Next(Int32 minValue, Int32 maxValue) {
			if (minValue > maxValue) throw new ArgumentOutOfRangeException("minValue");
			if (minValue == maxValue) return minValue; Int64 diff = maxValue - minValue;
			while (true) {
				_rng.GetBytes(_uint32Buffer);
				UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0);
				Int64 max = (1 + (Int64)UInt32.MaxValue);
				Int64 remainder = max % diff; if (rand < max - remainder) { 
					return (Int32)(minValue + (rand % diff)); 
				} 
			} 
		} public override double NextDouble() {
			_rng.GetBytes(_uint32Buffer);
			UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0);
			return rand / (1.0 + UInt32.MaxValue);
		} 
		public override void NextBytes(byte[] buffer) {
			if (buffer == null) throw new ArgumentNullException("buffer"); 
			_rng.GetBytes(buffer); 
		} 
	}
}

    }
