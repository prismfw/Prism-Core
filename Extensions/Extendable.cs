/*
Copyright (C) 2018  Prism Framework Team

This file is part of the Prism Framework.

The Prism Framework is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

The Prism Framework is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/


using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Prism.Extensions
{
	/// <summary>
	/// Provides additional methods for various object types.
	/// </summary>
    public static class Extendable
    {
        #region IDictionary<TKey, TValue>
        /// <summary>
        /// Merges the contents of the given dictionary into the current dictionary, overwriting any current values.
        /// </summary>
        /// <param name="source">The source dictionary.</param>
        /// <param name="dictionary">The dictionary to be merged.</param>
        /// <exception cref="ArgumentNullException">Thrown when either dictionary is <c>null</c>.</exception>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> dictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            foreach (var key in dictionary.Keys)
            {
                source[key] = dictionary[key];
            }
        }

        /// <summary>
        /// Merges the contents of the given dictionary into the current dictionary.
        /// </summary>
        /// <param name="source">The source dictionary.</param>
        /// <param name="dictionary">The dictionary to be merged.</param>
        /// <param name="overwrite">Whether to overwrite any existing values with identical keys.</param>
        /// <exception cref="ArgumentNullException">Thrown when either dictionary is <c>null</c>.</exception>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> dictionary, bool overwrite)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            foreach (var key in dictionary.Keys)
            {
                if (overwrite || !source.ContainsKey(key))
                {
                    source[key] = dictionary[key];
                }
            }
        }

        /// <summary>
        /// Removes the value associated with the specified key and returns it.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary&lt;TKey, TValue&gt;"/> object.</param>
        /// <param name="key">The key for the value to remove and return.</param>
        /// <returns>The value that was removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dictionary"/> is <c>null</c> -or- when <paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when <paramref name="key"/> does not exist within the dictionary.</exception>
        public static TValue Extract<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var item = dictionary[key];
            dictionary.Remove(key);
            return item;
        }

        /// <summary>
        /// Returns the value associated with the specified key, or returns the default value of TValue if the key was not found.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary&lt;TKey, TValue&gt;"/> object.</param>
        /// <param name="key">The key for the value to return.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
        /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
        /// <returns>The value associated with the key -or- the default value of TValue.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dictionary"/> is <c>null</c>.</exception>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (key == null)
            {
                return default(TValue);
            }

            TValue value;
            dictionary.TryGetValue(key, out value);
            return value;
        }

		/// <summary>
		///	Returns the value associated with the specified key, or returns the specified default value if the key was not found.
		/// </summary>
		/// <param name="dictionary">The <see cref="IDictionary&lt;TKey, TValue&gt;"/> object.</param>
		/// <param name="key">The key for the value to return.</param>
		/// <param name="defaultValue">The value to return if the key was not found.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		/// <typeparam name="TValue">The 2nd type parameter.</typeparam>
		/// <returns>The value associated with the key -or- the specified default value.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dictionary"/> is <c>null</c>.</exception>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (key == null)
            {
                return defaultValue;
            }

            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary&lt;TKey, TValue&gt;"/> object.</param>
        /// <param name="match">The delegate that defines the conditions of the elements to remove.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
		/// <typeparam name="TValue">The 2nd type parameter.</typeparam>
		/// <returns>The number of elements that have been removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dictionary"/> is <c>null</c> -or- when <paramref name="match"/> is <c>null</c>.</exception>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Signature is necessary to provide the expected functionality.  Typical use of this method will not expose the nested nature of the parameters.")]
        public static int RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> match)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            var keys = new List<TKey>();
            foreach (var kvp in dictionary)
            {
                if (match(kvp))
                {
                    keys.Add(kvp.Key);
                }
            }

            int count = 0;
            foreach (var key in keys)
            {
                if (dictionary.Remove(key))
                {
                    count++;
                }
            }

            return count;
        }
        #endregion

        #region IEnumerable<T>
        /// <summary>
        /// Determines whether two collections contain the same elements.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <param name="collection">The collection with which to compare elements.</param>
        /// <returns><c>true</c> if the collections contain the same elements; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c> -or- when <paramref name="collection"/> is <c>null</c>.</exception>
        public static bool IsEquivalent<T>(this IEnumerable<T> source, IEnumerable<T> collection)
        {
            return source.IsEquivalent(collection, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Determines whether two collections contain the same elements.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <param name="collection">The collection with which to compare elements.</param>
        /// <param name="comparer">An equality comparer to compare the elements within the collections.</param>
        /// <returns><c>true</c> if the collections contain the same elements; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c> -or- when <paramref name="collection"/> is <c>null</c>.</exception>
        public static bool IsEquivalent<T>(this IEnumerable<T> source, IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (source == collection)
            {
                return true;
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            var comparison = new HashSet<T>(comparer);
            var enumerator = collection.GetEnumerator();
            foreach (var value in source)
            {
                if (!enumerator.MoveNext())
                {
                    return false;
                }

                if (!comparer.Equals(value, enumerator.Current))
                {
                    if (!comparison.Remove(enumerator.Current))
                    {
                        comparison.Add(enumerator.Current);
                    }
                    if (!comparison.Remove(value))
                    {
                        comparison.Add(value);
                    }
                }
            }

            return !enumerator.MoveNext() && comparison.Count == 0;
        }

        /// <summary>
        /// Returns the element at a specified index in a sequence or a default value if the index is out of range.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="defaultValue">The value to return if the index is out of range.</param>
        /// <returns>The element at the specified index in the sequence, or <paramref name="defaultValue"/> if the index was outside the bounds of the sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        public static T ElementAtOrDefault<T>(this IEnumerable<T> source, int index, T defaultValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (index < 0)
            {
                return defaultValue;
            }

            var tList = source as IList<T>;
            if (tList != null)
            {
                return tList.Count > index ? tList[index] : defaultValue;
            }
            
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (index-- == 0)
                {
                    return enumerator.Current;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns the first element of a sequence or a default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="defaultValue">The value to return if the sequence is empty.</param>
        /// <returns>The first element in the sequence, or <paramref name="defaultValue"/> if the sequence is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        public static T FirstOrDefault<T>(this IEnumerable<T> source, T defaultValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns the first element of a sequence that satisfies a condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="defaultValue">The value to return if there are no matching elements in the sequence.</param>
        /// <returns>The first element in the sequence that satisfied the condition, or <paramref name="defaultValue"/> if no such element was found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c> -or- when <paramref name="predicate"/> is <c>null</c>.</exception>
        public static T FirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate, T defaultValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    return enumerator.Current;
                }
            }
            
            return defaultValue;
        }

        /// <summary>
        /// Returns the last element of a sequence or a default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="defaultValue">The value to return if the sequence is empty.</param>
        /// <returns>The last element in the sequence, or <paramref name="defaultValue"/> if the sequence is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        public static T LastOrDefault<T>(this IEnumerable<T> source, T defaultValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var tList = source as IList<T>;
            if (tList != null)
            {
                return tList.Count > 0 ? tList[tList.Count - 1] : defaultValue;
            }

            var retval = defaultValue;

            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                retval = enumerator.Current;
            }

            return retval;
        }

        /// <summary>
        /// Returns the last element of a sequence that satisfies a condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="defaultValue">The value to return if there are no matching elements in the sequence.</param>
        /// <returns>The last element in the sequence that satisfied the condition, or <paramref name="defaultValue"/> if no such element was found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c> -or- when <paramref name="predicate"/> is <c>null</c>.</exception>
        public static T LastOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate, T defaultValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var tList = source as IList<T>;
            if (tList != null)
            {
                if (tList.Count > 0)
                {
                    for (int i = tList.Count - 1; i >= 0; i--)
                    {
                        var element = tList[i];
                        if (predicate(element))
                        {
                            return element;
                        }
                    }
                }

                return defaultValue;
            }

            var retval = defaultValue;
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    retval = enumerator.Current;
                }
            }

            return retval;
        }

        /// <summary>
        /// Returns the only element of a sequence or a default value if the sequence is empty;
        /// this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="defaultValue">The value to return if there are no elements in the sequence.</param>
        /// <returns>The single element in the sequence, or <paramref name="defaultValue"/> if the sequence was empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when there is more than one element in the sequence.</exception>
        public static T SingleOrDefault<T>(this IEnumerable<T> source, T defaultValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return defaultValue;
            }

            T currentValue = enumerator.Current;
            if (enumerator.MoveNext())
            {
                throw new InvalidOperationException(Resources.Strings.MoreThanOneElementInSequence);
            }

            return currentValue;
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a condition or a default value if no such element exists;
        /// this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="defaultValue">The value to return if there are no matching elements in the sequence.</param>
        /// <returns>The single element that satisfied the condition, or <paramref name="defaultValue"/> if no such element was found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is <c>null</c> -or- when <paramref name="predicate"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when more than one element in the sequence satisfies the condition.</exception>
        public static T SingleOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate, T defaultValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            int count = 0;
            var retval = defaultValue;

            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    retval = enumerator.Current;
                    if (++count > 1)
                    {
                        throw new InvalidOperationException(Resources.Strings.MoreThanOneElementInSequence);
                    }
                }
            }

            return retval;
        }
        #endregion

        #region IList<T>
        /// <summary>
        /// Removes the element at the specified index and returns the element that was removed.
        /// </summary>
        /// <param name="list">The <see cref="IList&lt;T&gt;"/> object.</param>
        /// <param name="index">The zero-based index of the element to remove and return.</param>
        /// <returns>The element that was removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection.</exception>
        public static T Extract<T>(this IList<T> list, int index)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (index < 0 || list.Count <= index)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var item = list[index];
            list.RemoveAt(index);
            return item;
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate and returns them.
        /// </summary>
        /// <param name="list">The <see cref="IList&lt;T&gt;"/> object.</param>
        /// <param name="match">The delegate that defines the conditions of the elements to remove and return.</param>
        /// <returns>An <see cref="IList&lt;T&gt;"/> instance containing the elements that were removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is <c>null</c> -or- when <paramref name="match"/> is <c>null</c>.</exception>
        public static IList<T> ExtractAll<T>(this IList<T> list, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            var retVal = new List<T>();
            for (int i = 0; i < list.Count;)
            {
                var item = list[i];
                if (match.Invoke(item))
                {
                    retVal.Add(item);
                    list.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Removes a range of elements starting at the specified index and returns the elements that were removed.
        /// </summary>
        /// <param name="list">The <see cref="IList&lt;T&gt;"/> object.</param>
        /// <param name="index">The zero-based starting index of the range of elements to remove and return.</param>
        /// <param name="count">The number of elements to remove and return.</param>
        /// <returns>An <see cref="IList&lt;T&gt;"/> instance containing the elements that were removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> exceeds the upper or lower bound of the collection -or- when <paramref name="count"/> exceeds the upper bound of the collection.</exception>
        public static IList<T> ExtractRange<T>(this IList<T> list, int index, int count)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (index < 0 || list.Count <= index)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0 || list.Count < index + count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var retVal = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                retVal.Add(list[index]);
                list.RemoveAt(index);
            }

            return retVal;
        }
        #endregion

        #region Random
        /// <summary>
        /// Returns a random <c>true</c> or <c>false</c> value with even probability.
        /// </summary>
        /// <param name="random">The <see cref="Random"/> object.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="random"/> is <c>null</c>.</exception>
        /// <returns>A random <c>true</c> or <c>false</c> value.</returns>
        public static bool CoinToss(this Random random)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextDouble() <= 0.5;
        }

        /// <summary>
        /// Returns a random <c>true</c> or <c>false</c> value with the specified probability.
        /// </summary>
        /// <param name="random">The <see cref="Random"/> object.</param>
        /// <param name="probability">The probability that <c>true</c> is returned, as a percentage value between 0.0 and 1.0.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="random"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="probability"/> is not a number or is infinite.</exception>
        /// <returns>A random <c>true</c> or <c>false</c> value.</returns>
        public static bool CoinToss(this Random random, double probability)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            if (double.IsNaN(probability) || double.IsInfinity(probability))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNaNOrInfinity, nameof(probability));
            }

            if (probability <= 0)
            {
                return false;
            }

            if (probability >= 1)
            {
                return true;
            }

            return random.NextDouble() <= probability;
        }
        #endregion
    }
}
