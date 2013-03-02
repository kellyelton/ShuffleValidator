using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

// ReSharper disable CheckNamespace
namespace Shuffle.Net
// ReSharper restore CheckNamespace
{
    public class QuantumList<T> : IList<T>
    {
        private static readonly RNGCryptoServiceProvider _random = new RNGCryptoServiceProvider();
        private static byte[] _lrandom = new byte[4];
        private List<QuantumState<T>> _items;


        public int Count { get { return _items.Count; } }
        public bool IsReadOnly { get { return false; } }

        public T this[int i]
        {
            get { return Get(i); }
            set
            {
                var g = Get(i);
                _items[i].Item = value;
                _items[i].Observed = true;
            }
        }

        public IEnumerable<QuantumState<T>> UnknownItems
        {
            get { return _items.Where(x => !x.Observed); }
        }

        public QuantumList()
        {
            _items = new List<QuantumState<T>>();
        }

        public T Get(int index)
        {
            //TODO this can all be optimized...it works, but I'm sure I can make it faster.
            //Index way the fuck out of range, holy shit!
            if (_items.Count == 0 || index >= _items.Count || index < 0) throw new ArgumentOutOfRangeException(this.GetType().Name, "There are no more items.");
            //If that item has been observed already, return it.
            if (_items[index].Observed) return _items[index].Item;
            //Item hasn't been observed, so grab an item and assign it to the slot
            var unknownIndexes = new List<int>();
            var unknownItems = new List<QuantumState<T>>();
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Observed) continue;
                unknownIndexes.Add(i);
                unknownItems.Add(_items[i]);
            }
            var fIndex = RandomIntBetween(0, unknownIndexes.Count);
            var iIndex = RandomIntBetween(0, unknownItems.Count);
            var oldone = _items[fIndex];
            var newone = _items[iIndex];
            _items.Insert(fIndex, newone);
            _items.RemoveAt(fIndex + 1);
            _items.Add(oldone);
            newone.Observed = true;
            _items.RemoveAt(iIndex);
            return newone.Item;
        }

        public static int RandomIntBetween(int min, int max)
        {
            _random.GetBytes(_lrandom);
            Int32 num = BitConverter.ToInt32(_lrandom, 0) % max;
            num = Math.Abs(num);
            num = (num % (max - min + 1)) + min;
            return num;
        }

        public void Add(T item) { _items.Add(new QuantumState<T>(item, false)); }
        public void Add(T item, bool observed) { _items.Add(new QuantumState<T>(item, observed)); }
        public void ClearObservations() { foreach (var a in _items) a.Observed = false; }
        public void Clear() { _items.Clear(); }
        public bool Contains(T item) { return _items.Exists(x => x.Item.Equals(item)); }
        public void CopyTo(T[] array, int arrayIndex)
        {
            T[] ret = new T[Count];
            for (int i = 0; i < Count; i++)
                ret[i] = Get(i);
        }
        public bool Remove(T item) { return _items.RemoveAll(x => x.Item.Equals(item)) > 0; }

        public int IndexOf(T item)
        {
            return -1;
            //TODO Finish this, was too lazy to do so.
            var i = _items.SingleOrDefault(x => x.Item.Equals(item));
            if (!i.Observed)
            {

            }
            else
            {

            }
        }

        public void Insert(int index, T item) { _items.Insert(index, new QuantumState<T>(item, true)); }

        public void RemoveAt(int index)
        {
            Get(index);
            _items.RemoveAt(index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _items.Count; i++) yield return Get(i);
        }

        public List<T> ToList()
        {
            while (_items.Any(x => !x.Observed))
            {
                for (var i = 0; i < _items.Count; i++)
                {
                    Get(i);
                }
            }
            return _items.Select(x => x.Item).ToList();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}