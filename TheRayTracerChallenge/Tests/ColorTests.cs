using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class ColorTests
    {
        [Test]
        public void Adding_colors()
        {
            var c1 = new Color(0.9, 0.6, 0.75);
            var c2 = new Color(0.7, 0.1, 0.25);
            var sum = c1 + c2;
            Assert.AreEqual(new Color(1.6, 0.7, 1.0), sum);
        }

        [Test]
        public void Subtracting_colors()
        {
            var c1 = new Color(0.9, 0.6, 0.75);
            var c2 = new Color(0.7, 0.1, 0.25);
            var diff = c1 - c2;
            Assert.AreEqual(new Color(0.2, 0.5, 0.5), diff);
        }

        [Test]
        public void Multiplying_a_color_with_a_scalar()
        {
            var c = new Color(0.2, 0.3, 0.4);
            var scaled = c * 2;
            Assert.AreEqual(new Color(0.4, 0.6, 0.8), scaled);
        }

        [Test]
        public void Multiplying_colors()
        {
            var c1 = new Color(1, 0.2, 0.4);
            var c2 = new Color(0.9, 1, 0.1);
            var product = c1 * c2;
            Assert.AreEqual(new Color(0.9, 0.2, 0.04), product);
        }
    }
}
