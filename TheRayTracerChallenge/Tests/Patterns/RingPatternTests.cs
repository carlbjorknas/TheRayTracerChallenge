using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Patterns;

namespace TheRayTracerChallenge.Tests.Patterns
{
    [TestFixture]
    public class RingPatternTests
    {
        [Test]
        public void A_ring_should_extend_in_both_x_and_z()
        {
            var pattern = new RingPattern(Color.White, Color.Black);
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.AreEqual(Color.Black, pattern.ColorAt(Tuple.Point(1, 0, 0)));
            Assert.AreEqual(Color.Black, pattern.ColorAt(Tuple.Point(0, 0, 1)));
            Assert.AreEqual(Color.Black, pattern.ColorAt(Tuple.Point(0.708, 0, 0.708)));
        }
    }
}
