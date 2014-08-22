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
using System.Collections.Concurrent;

namespace Beerendonk.Memoization
{
    /// <summary>
    /// Memoize extension functions for Func types.
    /// </summary>
    public static class FuncExtensions
    {
        /// <summary>
        /// <para>
        /// Returns a memoized version of a referentially transparent function. The memoized version of the
        /// function keeps a cache of the mapping from arguments to results and, when calls with the same
        /// arguments are repeated often, has higher performance at the expense of higher memory use.
        /// </para>
        /// <para>
        /// A limitless cache is used.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function to memoize.</param>
        /// <returns>A memoized version of the function.</returns>
        /// <example>
        /// <code>
        /// Func&lt;int, int&gt; inc = x =&gt; x + 1;
        /// var memoizedInc = inc.Memoize();
        /// Console.WriteLine(memoizedInc(1));  // Calls inc and caches the result
        /// Console.WriteLine(memoizedInc(1));  // Reads the result from the cache
        /// </code>
        /// </example>
        public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
        {
            return Memoize(func, CacheFactory.LimitlessCache<T, TResult>());
        }

        /// <summary>
        /// Returns a memoized version of a referentially transparent function. The memoized version of the
        /// function keeps a cache of the mapping from arguments to results and, when calls with the same
        /// arguments are repeated often, has higher performance at the expense of higher memory use.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function to memoize.</param>
        /// <param name="cache">The cache to use.</param>
        /// <returns>A memoized version of the function.</returns>
        /// <example>
        /// <code>
        /// ICache&lt;int, int&gt; cache = CacheFactory.LimitlessCache&lt;int, int&gt;();
        /// Func&lt;int, int&gt; inc = x =&gt; x + 1;
        /// var memoizedInc = inc.Memoize();
        /// Console.WriteLine(memoizedInc(1));  // Calls inc and caches the result
        /// Console.WriteLine(memoizedInc(1));  // Reads the result from the cache
        /// </code>
        /// </example>
        public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func, ICache<T, TResult> cache)
        {
            return args => cache.GetOrAdd(args, func);
        }

        /// <summary>
        /// <para>
        /// Returns a memoized version of a referentially transparent function. The memoized version of the 
        /// function keeps a cache of the mapping from arguments to results and, when calls with the same 
        /// arguments are repeated often, has higher performance at the expense of higher memory use.
        /// </para>
        /// <para>
        /// A limitless cache is used.
        /// </para>
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function to memoize.</param>
        /// <returns>A memoized version of the function.</returns>
        /// <example>
        /// <code>
        /// Func&lt;int, int, int&gt; add = (x, y) =&gt; x + y;
        /// var memoizedAdd = add.Memoize();
        /// Console.WriteLine(memoizedAdd(1, 2));  // Calls add and caches the result
        /// Console.WriteLine(memoizedAdd(1, 2));  // Reads the result from the cache
        /// </code>
        /// </example>
        public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            return Memoize(func, CacheFactory.LimitlessCache<Tuple<T1, T2>, TResult>());
        }

        /// <summary>
        /// Returns a memoized version of a referentially transparent function. The memoized version of the 
        /// function keeps a cache of the mapping from arguments to results and, when calls with the same 
        /// arguments are repeated often, has higher performance at the expense of higher memory use.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function to memoize.</param>
        /// <param name="cache">The cache to use.</param>
        /// <returns>A memoized version of the function.</returns>
        /// <example>
        /// <code>
        /// ICache&lt;Tuple&lt;int, int&gt;, int&gt; cache = CacheFactory.LimitlessCache&lt;Tuple&lt;int, int&gt;, int&gt;();
        /// Func&lt;int, int, int&gt; add = (x, y) =&gt; x + y;
        /// var memoizedAdd = add.Memoize(cache);
        /// Console.WriteLine(memoizedAdd(1, 2));  // Calls add and caches the result
        /// Console.WriteLine(memoizedAdd(1, 2));  // Reads the result from the cache
        /// </code>
        /// </example>
        public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> func, ICache<Tuple<T1, T2>, TResult> cache)
        {
            return func.Tuplify().Memoize(cache).Detuplify();
        }

        private static Func<Tuple<T1, T2>, TResult> Tuplify<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            return t => func(t.Item1, t.Item2);
        }

        private static Func<T1, T2, TResult> Detuplify<T1, T2, TResult>(this Func<Tuple<T1, T2>, TResult> func)
        {
            return (arg1, arg2) => func(Tuple.Create(arg1, arg2));
        }

        /// <summary>
        /// <para>
        /// Returns a memoized version of a referentially transparent function. The memoized version of the 
        /// function keeps a cache of the mapping from arguments to results and, when calls with the same 
        /// arguments are repeated often, has higher performance at the expense of higher memory use.
        /// </para>
        /// <para>
        /// A limitless cache is used.
        /// </para>
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function to memoize.</param>
        /// <returns>A memoized version of the function.</returns>
        /// <example>
        /// <code>
        /// Func&lt;int, int, int, int&gt; add = (x, y, z) =&gt; x + y + z;
        /// var memoizedAdd = add.Memoize();
        /// Console.WriteLine(memoizedAdd(1, 2, 3));  // Calls add and caches the result
        /// Console.WriteLine(memoizedAdd(1, 2, 3));  // Reads the result from the cache
        /// </code>
        /// </example>
        public static Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        {
            return Memoize(func, CacheFactory.LimitlessCache<Tuple<T1, T2, T3>, TResult>());
        }

        /// <summary>
        /// Returns a memoized version of a referentially transparent function. The memoized version of the 
        /// function keeps a cache of the mapping from arguments to results and, when calls with the same 
        /// arguments are repeated often, has higher performance at the expense of higher memory use.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function to memoize.</param>
        /// <param name="cache">The cache to use.</param>
        /// <returns>A memoized version of the function.</returns>
        /// <example>
        /// <code>
        /// ICache&lt;Tuple&lt;int, int, int&gt;, int&gt; cache = CacheFactory.LimitlessCache&lt;Tuple&lt;int, int, int&gt;, int&gt;();
        /// Func&lt;int, int, int, int&gt; add = (x, y, z) =&gt; x + y + z;
        /// var memoizedAdd = add.Memoize(cache);
        /// Console.WriteLine(memoizedAdd(1, 2));  // Calls add and caches the result
        /// Console.WriteLine(memoizedAdd(1, 2));  // Reads the result from the cache
        /// </code>
        /// </example>
        public static Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, ICache<Tuple<T1, T2, T3>, TResult> cache)
        {
            return func.Tuplify().Memoize(cache).Detuplify();
        }

        private static Func<Tuple<T1, T2, T3>, TResult> Tuplify<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        {
            return t => func(t.Item1, t.Item2, t.Item3);
        }

        private static Func<T1, T2, T3, TResult> Detuplify<T1, T2, T3, TResult>(this Func<Tuple<T1, T2, T3>, TResult> func)
        {
            return (arg1, arg2, arg3) => func(Tuple.Create(arg1, arg2, arg3));
        }
    }
}
