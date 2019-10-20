using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Patterns;

namespace TheRayTracerChallenge.Tests.Patterns
{
    [TestFixture]
    class StripePatternTests
    {
        [Test]
        public void Stripe_pattern_is_a_pattern()
        {
            var pattern = new StripePattern(Color.White, Color.Black);
            Assert.IsTrue(pattern is Pattern);
        }

        [Test]
        public void Creating_a_stripe_pattern()
        {
            var pattern = new StripePattern(Color.White, Color.Black);
            Assert.AreEqual(Color.White, pattern.Color1);
            Assert.AreEqual(Color.Black, pattern.Color2);
        }

        [Test]
        public void A_stripe_pattern_is_constant_in_y()
        {
            var pattern = new StripePattern(Color.White, Color.Black);
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 1, 0)));
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 2, 0)));
        }

        [Test]
        public void A_stripe_pattern_is_constant_in_z()
        {
            var pattern = new StripePattern(Color.White, Color.Black);
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 0, 1)));
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 0, 2)));
        }

        [Test]
        public void A_stripe_pattern_alternates_in_x()
        {
            var pattern = new StripePattern(Color.White, Color.Black);
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(0.9, 0, 0)));
            Assert.AreEqual(Color.Black, pattern.ColorAt(Tuple.Point(1, 0, 0)));
            Assert.AreEqual(Color.Black, pattern.ColorAt(Tuple.Point(-0.1, 0, 0)));
            Assert.AreEqual(Color.Black, pattern.ColorAt(Tuple.Point(-1, 0, 0)));
            Assert.AreEqual(Color.White, pattern.ColorAt(Tuple.Point(-1.1, 0, 0)));
        }
    }
}
