using System;
using System.Collections.Generic;
using System.Linq;
using MMR.Common.Utils;

namespace MMR.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
                elements.SelectMany((e, i) =>
                    elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        // todo move to RandomExtensions
        public static T RandomOrDefault<T>(this IList<T> list, Random random)
        {
            return list.Any() ? list[random.Next(list.Count)] : default(T);
        }

        // todo move to RandomExtensions
        public static T Random<T>(this IList<T> list, Random random)
        {
            if (!list.Any())
            {
                throw new Exception("List is empty.");
            }
            if (list.Count == 1)
            {
                return list[0];
            }
            return list[random.Next(list.Count)];
        }

        // todo move to RandomExtensions
        /// <summary>
        /// Select unique random items from a given array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">Source items list</param>
        /// <param name="amount">Selection amount</param>
        /// <param name="random">Random</param>
        /// <returns>Selected items</returns>
        public static T[] Random<T>(this IList<T> list, int amount, Random random)
        {
            if (amount > list.Count)
            {
                throw new IndexOutOfRangeException("Selection amount cannot exceed array length.");
            }

            var result = new List<T>(amount);
            var source = list.ToList();
            for (int i = 0; i < amount; i++)
            {
                var index = random.Next(source.Count);
                var selected = source[index];
                result.Add(selected);
                source.RemoveAt(index);
            }
            return result.ToArray();
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.Distinct(new KeyEqualityComparer<TSource, TKey>(keySelector));
        }

        public static IEnumerable<T> AllowModification<T>(this IEnumerable<T> source) where T : struct
        {
            var used = new List<T>();
            for (var item = source.Cast<T?>().FirstOrDefault(); item != null; used.Add(item.Value), item = source.Except(used).Cast<T?>().FirstOrDefault())
            {
                yield return item.Value;
            }
        }
    }
}
