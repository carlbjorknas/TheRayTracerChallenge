using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class LightTests
    {
        [Test]
        public void A_point_light_has_a_position_and_intensity()
        {
            var intensity = new Color(1, 1, 1);
            var position = Tuple.Point(0, 0, 0);

            var light = new PointLight(position, intensity);

            Assert.AreEqual(position, light.Position);
            Assert.AreEqual(intensity, light.Intensity);
        }
    }
}
