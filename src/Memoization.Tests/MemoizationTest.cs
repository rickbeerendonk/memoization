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
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Beerendonk.Memoization;

namespace Beerendonk.Memoization.Tests
{
    [TestClass]
    public class MemoizationTest
    {
        [TestMethod]
        public void Memoize_with_single_argument_should_return_the_correct_value()
        {
            // Arrange
            Func<int, int> inc = x => x + 1;
            var incMemoized = FuncExtensions.Memoize(inc);

            // Act
            incMemoized(1);
            int result = incMemoized(1);

            // Assert
            result.Should().Be(2);
        }

        [TestMethod]
        public void Memoize_with_single_argument_should_use_cached_value_for_same_arguments()
        {
            // Arrange
            int count = 0;
            Func<int, int> inc = x =>
            {
                count++;
                return x + 1;
            };
            var incMemoized = FuncExtensions.Memoize(inc);

            // Act
            incMemoized(1);
            incMemoized(1);

            // Assert
            count.Should().Be(1);
        }

        [TestMethod]
        public void Memoize_with_single_argument_should_not_use_cached_value_for_new_arguments()
        {
            // Arrange
            int count = 0;
            Func<int, int> inc = x =>
            {
                count++;
                return x + 1;
            };
            var incMemoized = FuncExtensions.Memoize(inc);

            // Act
            incMemoized(1);
            int result = incMemoized(2);

            // Assert
            count.Should().Be(2);
            result.Should().Be(3);
        }

        [TestMethod]
        public void Memoize_with_single_argument_and_cache_should_use_the_cache()
        {
            // Arrange
            Func<int, int> inc = x => x + 1;
            var cacheMock = new Mock<ICache<int, int>>();
            cacheMock.Setup(m => m.GetOrAdd(1, inc)).Returns(3);
            var incMemoized = FuncExtensions.Memoize(inc, cacheMock.Object);

            // Act
            int result = incMemoized(1);

            // Assert
            result.Should().Be(3);
            cacheMock.Verify(m => m.GetOrAdd(1, inc));
        }

        [TestMethod]
        public void Memoize_with_two_arguments_should_return_the_correct_value()
        {
            // Arrange
            Func<int, int, int> add = (x, y) => x + y;
            var addMemoized = FuncExtensions.Memoize(add);

            // Act
            addMemoized(1, 2);
            int result = addMemoized(1, 2);

            // Assert
            result.Should().Be(3);
        }

        [TestMethod]
        public void Memoize_with_two_arguments_should_use_cached_value_for_same_arguments()
        {
            // Arrange
            int count = 0;
            Func<int, int, int> add = (x, y) =>
            {
                count++;
                return x + y;
            };
            var addMemoized = FuncExtensions.Memoize(add);

            // Act
            addMemoized(1, 2);
            addMemoized(1, 2);

            // Assert
            count.Should().Be(1);
        }

        [TestMethod]
        public void Memoize_with_two_arguments_should_not_use_cached_value_for_new_arguments()
        {
            // Arrange
            int count = 0;
            Func<int, int, int> add = (x, y) =>
            {
                count++;
                return x + y;
            };
            var addMemoized = FuncExtensions.Memoize(add);

            // Act
            addMemoized(1, 2);
            int result = addMemoized(2, 3);

            // Assert
            count.Should().Be(2);
            result.Should().Be(5);
        }

        [TestMethod]
        public void Memoize_with_two_arguments_and_cache_should_use_the_cache()
        {
            // Arrange
            Tuple<int, int> key = Tuple.Create(1, 2);
            Func<int, int, int> add = (x, y) => x + y;
            var cacheMock = new Mock<ICache<Tuple<int, int>, int>>();
            cacheMock.Setup(m => m.GetOrAdd(key, It.IsAny<Func<Tuple<int, int>, int>>())).Returns(-1);
            var addMemoized = FuncExtensions.Memoize(add, cacheMock.Object);

            // Act
            int result = addMemoized(1, 2);

            // Assert
            result.Should().Be(-1);
            cacheMock.Verify(m => m.GetOrAdd(key, It.IsAny<Func<Tuple<int, int>, int>>()));
        }

        [TestMethod]
        public void Memoize_with_three_arguments_should_return_the_correct_value()
        {
            // Arrange
            Func<int, int, int, int> add = (x, y, z) => x + y + z;
            var addMemoized = FuncExtensions.Memoize(add);

            // Act
            addMemoized(1, 2, 3);
            int result = addMemoized(1, 2, 3);

            // Assert
            result.Should().Be(6);
        }

        [TestMethod]
        public void Memoize_with_three_arguments_should_use_cached_value_for_same_arguments()
        {
            // Arrange
            int count = 0;
            Func<int, int, int, int> add = (x, y, z) =>
            {
                count++;
                return x + y + z;
            };
            var addMemoized = FuncExtensions.Memoize(add);

            // Act
            addMemoized(1, 2, 3);
            addMemoized(1, 2, 3);

            // Assert
            count.Should().Be(1);
        }

        [TestMethod]
        public void Memoize_with_three_arguments_should_not_use_cached_value_for_new_arguments()
        {
            // Arrange
            int count = 0;
            Func<int, int, int, int> add = (x, y, z) =>
            {
                count++;
                return x + y + z;
            };
            var addMemoized = FuncExtensions.Memoize(add);

            // Act
            addMemoized(1, 2, 3);
            int result = addMemoized(2, 3, 4);

            // Assert
            count.Should().Be(2);
            result.Should().Be(9);
        }

        [TestMethod]
        public void Memoize_with_three_arguments_and_cache_should_use_the_cache()
        {
            // Arrange
            Tuple<int, int, int> key = Tuple.Create(1, 2, 3);
            Func<int, int, int, int> add = (x, y, z) => x + y + z;
            var cacheMock = new Mock<ICache<Tuple<int, int, int>, int>>();
            cacheMock.Setup(m => m.GetOrAdd(key, It.IsAny<Func<Tuple<int, int, int>, int>>())).Returns(-1);
            var addMemoized = FuncExtensions.Memoize(add, cacheMock.Object);

            // Act
            int result = addMemoized(1, 2, 3);

            // Assert
            result.Should().Be(-1);
            cacheMock.Verify(m => m.GetOrAdd(key, It.IsAny<Func<Tuple<int, int, int>, int>>()));
        }
    }
}
