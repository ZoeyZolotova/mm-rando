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
        public static T RandomOrDefault<T>(this IList<T> list, Random random, Func<T, double> weight = null)
        {
            return list.Any() ? list.Random(random, weight) : default(T);
        }

        // todo move to RandomExtensions
        public static T Random<T>(this IList<T> list, Random random, Func<T, double> weight = null)
        {
            if (!list.Any())
            {
                throw new Exception("List is empty.");
            }
            if (list.Count == 1)
            {
                return list[0];
            }
            if (weight == null)
            {
                return list[random.Next(list.Count)];
            }

            var totalWeight = list.Sum(weight);

            if (totalWeight == 0)
            {
                return list[random.Next(list.Count)];
            }

            var selectedWeight = random.NextDouble() * totalWeight;
            double currentWeight = 0;
            
            foreach (var item in list)
            {
                currentWeight += weight(item);
                if (currentWeight > selectedWeight)
                {
                    return item;
                }
            }

            throw new Exception("Failed to select from weighted random list. This shouldn't happen.");
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
                amount = list.Count;
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

        /// <summary>
        /// Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
        /// </summary>
        /// <remarks>
        /// Every chunk except the last will be of size <paramref name="size"/>.
        /// The last chunk will contain the remaining elements and may be of a smaller size.
        /// </remarks>
        /// <param name="source">
        /// An <see cref="IEnumerable{T}"/> whose elements to chunk.
        /// </param>
        /// <param name="size">
        /// Maximum size of each chunk.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that contains the elements the input sequence split into chunks of size <paramref name="size"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="size"/> is below 1.
        /// </exception>
        //public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> source, int size)
        //{
        //    if (source == null)
        //    {
        //        throw new ArgumentNullException(nameof(source));
        //    }

        //    if (size < 1)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(size));
        //    }

        //    return ChunkIterator(source, size);
        //}

        private static IEnumerable<TSource[]> ChunkIterator<TSource>(IEnumerable<TSource> source, int size)
        {
            using IEnumerator<TSource> e = source.GetEnumerator();

            if (e.MoveNext())
            {
                List<TSource> chunkBuilder = new();
                while (true)
                {
                    do
                    {
                        chunkBuilder.Add(e.Current);
                    }
                    while (chunkBuilder.Count < size && e.MoveNext());

                    yield return chunkBuilder.ToArray();

                    if (chunkBuilder.Count < size || !e.MoveNext())
                    {
                        yield break;
                    }
                    chunkBuilder.Clear();
                }
            }
        }

        public static byte[] FindAndReplace(this byte[] src, byte[] find, byte[] replace)
        {
            var result = new List<byte>();
            var matches = FindBytes(src, find);
            var startIndex = 0;
            for (var i = 0; i < matches.Count; i++)
            {
                var index = matches[i];
                result.AddRange(src.Skip(startIndex).Take(index - startIndex));
                result.AddRange(replace);
                startIndex = index + find.Length;
            }
            result.AddRange(src.Skip(startIndex).Take(src.Length - startIndex));

            return result.ToArray();
        }

        private static List<int> FindBytes(byte[] src, byte[] find)
        {
            int matchIndex = 0;
            var matches = new List<int>();
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        matches.Add(i - matchIndex);
                        matchIndex = 0;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return matches;
        }
    }
}
