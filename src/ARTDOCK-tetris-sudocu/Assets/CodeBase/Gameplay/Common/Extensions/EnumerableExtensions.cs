using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static T PickRandom<T>(this IEnumerable<T> collection, IEnumerable<T> exclusions)
        {
            var excluded = exclusions.ToHashSet();

            var items = collection
                .Where(x => !excluded.Contains(x))
                .ToArray();

            if (items.Length > 0)
                return items[Random.Range(0, items.Length)];

            if (excluded.Count > 0)
                return excluded.ElementAt(Random.Range(0, excluded.Count));

            throw new InvalidOperationException(
                "Cannot pick a random element from an empty collection.");
        }
    }
}