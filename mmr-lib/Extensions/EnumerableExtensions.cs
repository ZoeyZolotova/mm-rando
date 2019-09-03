using System;
using System.Collections.Generic;
using System.Linq;

namespace MMRando.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
                elements.SelectMany((e, i) =>
                    elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        // todo move to ListExtensions
        public static T RandomOrDefault<T>(this IList<T> list, Random random)
        {
            return list.Any() ? list[random.Next(list.Count)] : default(T);
        }

        // todo move to ListExtensions
        public static T Random<T>(this IList<T> list, Random random)
        {
            if (!list.Any())
            {
                throw new InvalidOperationException("List is empty.");
            }
            if (list.Count == 1)
            {
                return list[0];
            }
            return list[random.Next(list.Count)];
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.Distinct(new KeyEqualityComparer<TSource, TKey>(keySelector));
        }
    }
}
