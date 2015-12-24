﻿// ---------------------------------------------------------------------------------------
// <copyright file="CustomTests.cs" company="Corale">
//     Copyright © 2015 by Adam Hellberg and Brandon Scott.
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy of
//     this software and associated documentation files (the "Software"), to deal in
//     the Software without restriction, including without limitation the rights to
//     use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//     of the Software, and to permit persons to whom the Software is furnished to do
//     so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//     WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//     CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//     "Razer" is a trademark of Razer USA Ltd.
// </copyright>
// ---------------------------------------------------------------------------------------

namespace Corale.Colore.Tests.Razer.Keyboard.Effects
{
    using System;

    using Corale.Colore.Core;
    using Corale.Colore.Razer.Keyboard;
    using Corale.Colore.Razer.Keyboard.Effects;

    using NUnit.Framework;

    [TestFixture]
    public class CustomTests
    {
        [Test]
        public void ShouldThrowWhenOutOfRangeGet()
        {
            var grid = Custom.Create();

            // ReSharper disable once NotAccessedVariable
            Color dummy;

            Assert.That(
                () => dummy = grid[-1, 0],
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName")
                      .EqualTo("row")
                      .And.Property("ActualValue")
                      .EqualTo(-1));

            Assert.That(
                () => dummy = grid[Constants.MaxRows + 1, 0],
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName")
                      .EqualTo("row")
                      .And.Property("ActualValue")
                      .EqualTo(Constants.MaxRows + 1));

            Assert.That(
                () => dummy = grid[0, -1],
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName")
                      .EqualTo("column")
                      .And.Property("ActualValue")
                      .EqualTo(-1));

            Assert.That(
                () => dummy = grid[0, Constants.MaxColumns + 1],
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName")
                      .EqualTo("column")
                      .And.Property("ActualValue")
                      .EqualTo(Constants.MaxColumns + 1));
        }

        [Test]
        public void ShouldThrowWhenOutOfRangeSet()
        {
            var grid = Custom.Create();

            Assert.That(
                () => grid[-1, 0] = Color.Red,
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName")
                      .EqualTo("row")
                      .And.Property("ActualValue")
                      .EqualTo(-1));

            Assert.That(
                () => grid[Constants.MaxRows + 1, 0] = Color.Red,
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName")
                      .EqualTo("row")
                      .And.Property("ActualValue")
                      .EqualTo(Constants.MaxRows + 1));

            Assert.That(
                () => grid[0, -1] = Color.Red,
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName")
                      .EqualTo("column")
                      .And.Property("ActualValue")
                      .EqualTo(-1));

            Assert.That(
                () => grid[0, Constants.MaxColumns + 1] = Color.Red,
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName")
                      .EqualTo("column")
                      .And.Property("ActualValue")
                      .EqualTo(Constants.MaxColumns + 1));
        }

        [Test]
        public void ShouldThrowWhenInvalidRowCount()
        {
            // We don't need to set up the columns as the code should throw before
            // it reaches the point of iterating rows
            var arr = new Color[2][];

            // ReSharper disable once NotAccessedVariable
            Custom dummy;

            Assert.That(
                () => dummy = new Custom(arr),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("colors"));
        }

        [Test]
        public void ShouldThrowWhenInvalidColumnCount()
        {
            var arr = new Color[Constants.MaxRows][];

            // We only need to populate one of the rows, as the
            // code shouldn't check further anyway.
            arr[0] = new Color[2];

            // ReSharper disable once NotAccessedVariable
            Custom dummy;

            Assert.That(
                () => dummy = new Custom(arr),
                Throws.ArgumentException.With.Property("ParamName").EqualTo("colors"));
        }

        [Test]
        public void ShouldSetToBlackWithCreate()
        {
            var grid = Custom.Create();

            for (var row = 0; row < Constants.MaxRows; row++)
            {
                for (var column = 0; column < Constants.MaxColumns; column++)
                    Assert.That(grid[row, column], Is.EqualTo(Color.Black));
            }
        }

        [Test]
        public void ShouldSetAllColorsWithColorCtor()
        {
            var grid = new Custom(Color.Red);

            for (var row = 0; row < Constants.MaxRows; row++)
            {
                for (var column = 0; column < Constants.MaxColumns; column++)
                    Assert.That(grid[row, column], Is.EqualTo(Color.Red));
            }
        }

        [Test]
        public void ShouldSetProperColorsWithArrCtor()
        {
            var arr = new Color[Constants.MaxRows][];

            for (var row = 0; row < Constants.MaxRows; row++)
                arr[row] = new Color[Constants.MaxColumns];

            // Set some arbitrary colors to test
            arr[0][5] = Color.Purple;
            arr[2][3] = Color.Pink;
            arr[4][0] = Color.Blue;

            var grid = new Custom(arr);

            for (var row = 0; row < Constants.MaxRows; row++)
            {
                for (var col = 0; col < Constants.MaxColumns; col++)
                    Assert.That(grid[row, col], Is.EqualTo(arr[row][col]));
            }
        }

        [Test]
        public void ShouldSetNewColors()
        {
            var grid = Custom.Create();

            grid[0, 5] = Color.Red;

            Assert.That(grid[0, 5], Is.EqualTo(Color.Red));
        }

        [Test]
        public void ShouldClearToBlack()
        {
            var grid = new Custom(Color.Pink);
            grid.Clear();

            Assert.That(grid, Is.EqualTo(Custom.Create()));
        }

        [Test]
        public void ShouldEqualIdenticalGrid()
        {
            var a = new Custom(Color.Red);
            var b = new Custom(Color.Red);

            Assert.True(a == b);
            Assert.False(a != b);
            Assert.True(a.Equals(b));
            Assert.AreEqual(a, b);
        }

        [Test]
        public void ShouldNotEqualDifferentGrid()
        {
            var a = new Custom(Color.Red);
            var b = new Custom(Color.Pink);

            Assert.False(a == b);
            Assert.True(a != b);
            Assert.False(a.Equals(b));
            Assert.AreNotEqual(a, b);
        }

        [Test]
        public void ShouldEqualIdenticalArray()
        {
            var grid = new Custom(Color.Red);
            var arr = new Color[Constants.MaxRows][];

            // Populate the 2D array
            for (var row = 0; row < Constants.MaxRows; row++)
            {
                arr[row] = new Color[Constants.MaxColumns];
                for (var col = 0; col < Constants.MaxColumns; col++)
                    arr[row][col] = Color.Red;
            }

            Assert.True(grid == arr);
            Assert.False(grid != arr);
            Assert.True(grid.Equals(arr));
            Assert.AreEqual(grid, arr);
        }

        [Test]
        public void ShouldNotEqualDifferentArray()
        {
            var grid = new Custom(Color.Pink);
            var arr = new Color[Constants.MaxRows][];

            // Populate the 2D array
            for (var row = 0; row < Constants.MaxRows; row++)
            {
                arr[row] = new Color[Constants.MaxColumns];
                for (var col = 0; col < Constants.MaxColumns; col++)
                    arr[row][col] = Color.Red;
            }

            Assert.False(grid == arr);
            Assert.True(grid != arr);
            Assert.False(grid.Equals(arr));
            Assert.AreNotEqual(grid, arr);
        }

        [Test]
        public void ShouldNotEqualArrayWithInvalidRowCount()
        {
            var grid = Custom.Create();
            var arr = new Color[2][];

            Assert.False(grid == arr);
            Assert.True(grid != arr);
            Assert.False(grid.Equals(arr));
            Assert.AreNotEqual(grid, arr);
        }

        [Test]
        public void ShouldNotEqualArrayWithInvalidColumnCount()
        {
            var grid = Custom.Create();
            var arr = new Color[Constants.MaxRows][];
            arr[0] = new Color[2];

            Assert.False(grid == arr);
            Assert.True(grid != arr);
            Assert.False(grid.Equals(arr));
            Assert.AreNotEqual(grid, arr);
        }

        [Test]
        public void ShouldNotEqualArbitraryObject()
        {
            var grid = Custom.Create();
            var obj = new object();

            Assert.False(grid == obj);
            Assert.True(grid != obj);
            Assert.False(grid.Equals(obj));
            Assert.AreNotEqual(grid, obj);
        }

        [Test]
        public void ShouldNotEqualNull()
        {
            var grid = Custom.Create();

            Assert.False(grid == null);
            Assert.True(grid != null);
            Assert.False(grid.Equals(null));
            Assert.AreNotEqual(grid, null);
        }
    }
}