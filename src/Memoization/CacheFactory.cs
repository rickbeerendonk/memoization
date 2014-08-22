// Copyright (c) Rick Beerendonk. All rights reserved.
//
// The use and distribution terms for this software are covered by the
// Eclipse Public License 1.0 (http://opensource.org/licenses/eclipse-1.0.php)
// which can be found in the file epl-v10.html at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

namespace Beerendonk.Memoization
{
    /// <summary>
    /// A factory class able to create <see cref="ICache{TKey, TValue}" /> instances.
    /// </summary>
    public static class CacheFactory
    {
        /// <summary>
        /// Creates a naive cache without memory limits.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>An infinite cache.</returns>
        public static ICache<TKey, TValue> LimitlessCache<TKey, TValue>()
        {
            return new LimitlessCache<TKey, TValue>();
        }
    }
}
