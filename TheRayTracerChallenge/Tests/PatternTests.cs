using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    class PatternTests
    {
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
            Assert.AreEqual(Color.White, pattern.StripeAt(Tuple.Point(0, 0, 0)));
            Assert.AreEqual(Color.White, pattern.StripeAt(Tuple.Point(0, 1, 0)));
            Assert.AreEqual(Color.White, pattern.StripeAt(Tuple.Point(0, 2, 0)));
        }

        [Test]
        public void A_stripe_pattern_is_constant_in_z()
        {
            var pattern = new StripePattern(Color.White, Color.Black);
            Assert.AreEqual(Color.White, pattern.StripeAt(Tuple.Point(0, 0, 0)));
            Assert.AreEqual(Color.White, pattern.StripeAt(Tuple.Point(0, 0, 1)));
            Assert.AreEqual(Color.White, pattern.StripeAt(Tuple.Point(0, 0, 2)));
        }

        [Test]
        public void A_stripe_pattern_alternates_in_x()
        {
            var pattern = new StripePattern(Color.White, Color.Black);
            Assert.AreEqual(Color.White, pattern.StripeAt(Tuple.Point(0, 0, 0)));
            Assert.AreEqual(Color.White, pattern.StripeAt(Tuple.Point(0.9, 0, 0)));
            Assert.AreEqual(Color.Black, pattern.StripeAt(Tuple.Point(1, 0, 0)));
            Assert.AreEqual(Color.Black, pattern.StripeAt(Tuple.Point(-0.1, 0, 0)));
            Assert.AreEqual(Color.Black, pattern.StripeAt(Tuple.Point(-1, 0, 0)));
            Assert.AreEqual(Color.White, pattern.StripeAt(Tuple.Point(-1.1, 0, 0)));
        }

        [Test]
        public void Stripes_with_an_object_transformation()
        {
            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Scaling(2, 2, 2);
            var pattern = new StripePattern(Color.White, Color.Black);
            var color = pattern.StripeAtObject(sphere, Tuple.Point(1.5, 0, 0));

            Assert.AreEqual(Color.White, color);
        }

        [Test]
        public void Stripes_with_a_pattern_transformation()
        {
            var sphere = Sphere.UnitSphere();            
            var pattern = new StripePattern(Color.White, Color.Black);
            pattern.Transform = Transformation.Scaling(2, 2, 2);
            var color = pattern.StripeAtObject(sphere, Tuple.Point(1.5, 0, 0));

            Assert.AreEqual(Color.White, color);
        }

        [Test]
        public void Stripes_with_both_an_object_and_a_pattern_transformation()
        {
            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Scaling(2, 2, 2);
            var pattern = new StripePattern(Color.White, Color.Black);
            pattern.Transform = Transformation.Translation(0.5, 0, 0);
            var color = pattern.StripeAtObject(sphere, Tuple.Point(2.5, 0, 0));

            Assert.AreEqual(Color.White, color);
        }
    }
}
