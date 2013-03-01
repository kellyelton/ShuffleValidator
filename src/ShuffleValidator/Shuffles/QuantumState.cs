// ReSharper disable CheckNamespace
namespace Shuffle.Net
// ReSharper restore CheckNamespace
{
    public class QuantumState<T>
    {
        public T Item { get; set; }
        public bool Observed { get; set; }

        public QuantumState(T item, bool observed = false)
        {
            Item = item;
            Observed = observed;
        }
    }
}