using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Patterns;

namespace TheRayTracerChallenge.Tests.Patterns
{
    [TestFixture]
    class CheckerPatternTests
    {
        [Test]
        public void Checkers_should_repeat_in_x()
        {
            var pattern = new CheckerPattern(Color.White, Color.Black);
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0.99, 0, 0)));
            Assert.AreEqual(Color.Black, pattern.ColorAt(Tuple.Point(1.01, 0, 0)));
        }

        [Test]
        public void Checkers_should_repeat_in_y()
        {
            var pattern = new CheckerPattern(Color.White, Color.Black);
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 0.99, 0)));
            Assert.AreEqual(Color.Black, pattern.ColorAt(Tuple.Point(0, 1.01, 0)));
        }

        [Test]
        public void Checkers_should_repeat_in_z()
        {
            var pattern = new CheckerPattern(Color.White, Color.Black);
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 0, 0.99)));
            Assert.AreEqual(Color.Black, pattern.ColorAt(Tuple.Point(0, 0, 1.01)));
        }
    }
}
