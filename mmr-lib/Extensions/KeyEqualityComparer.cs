using System;
using System.Collections.Generic;

namespace MMRando.Extensions
{
    public class KeyEqualityComparer<TSource, TKey> : IEqualityComparer<TSource>
    {
        private readonly Func<TSource, TKey> _keySelector;
        private readonly IEqualityComparer<TKey> _comparer;

        public KeyEqualityComparer(Func<TSource, TKey> keySelector)
        {
            _keySelector = keySelector;
            _comparer = EqualityComparer<TKey>.Default;
        }

        public bool Equals(TSource x, TSource y)
        {
            return _comparer.Equals(_keySelector(x), _keySelector(y));
        }

        public int GetHashCode(TSource obj)
        {
            return _comparer.GetHashCode(_keySelector(obj));
        }
    }
}
