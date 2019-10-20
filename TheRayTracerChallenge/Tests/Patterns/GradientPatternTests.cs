using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Patterns;

namespace TheRayTracerChallenge.Tests.Patterns
{
    [TestFixture]
    class GradientPatternTests
    {
        [Test]
        public void A_gradient_linearly_interpolates_between_colors()
        {
            var pattern = new GradientPattern(Color.White, Color.Black);
            Assert.AreEqual(
                new Color(1, 1, 1), 
                pattern.ColorAt(Tuple.Point(0, 0, 0)));
            Assert.AreEqual(
                new Color(0.75, 0.75, 0.75), 
                pattern.ColorAt(Tuple.Point(0.25, 0, 0)));
            Assert.AreEqual(
                new Color(0.5, 0.5, 0.5), 
                pattern.ColorAt(Tuple.Point(0.5, 0, 0)));
            Assert.AreEqual(
                new Color(0.25, 0.25, 0.25), 
                pattern.ColorAt(Tuple.Point(0.75, 0, 0)));
        }
    }
}
