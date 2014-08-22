// Copyright (c) Rick Beerendonk. All rights reserved.
//
// The use and distribution terms for this software are covered by the
// Eclipse Public License 1.0 (http://opensource.org/licenses/eclipse-1.0.php)
// which can be found in the file epl-v10.html at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;

namespace Beerendonk.Memoization
{
    /// <summary>
    /// Identifies a key-value-cache.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface ICache<TKey, TValue>
    {
        /// <summary>Removes all keys and values from the cache.</summary>
        void Clear();

        /// <summary>Adds a key/value pair to the cache if the key does not already exist.</summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">The function used to generate a value for the key.</param>
        /// <returns>
        /// The value for the key. This will be either the existing value for the key if 
        /// the key is already in the cache, or the new value for the key as returned by 
        /// valueFactory if the key was not in the cache.
        /// </returns>
        TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);
    }
}
