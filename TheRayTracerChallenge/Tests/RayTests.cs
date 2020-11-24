using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class RayTests
    {
        [Test]
        public void Computing_a_point_from_a_distance()
        {
            var ray = new Ray(
                Tuple.Point(2, 3, 4), 
                Tuple.Vector(1, 0, 0));

            Assert.AreEqual(Tuple.Point(2, 3, 4), ray.Position(0));
            Assert.AreEqual(Tuple.Point(3, 3, 4), ray.Position(1));
            Assert.AreEqual(Tuple.Point(1, 3, 4), ray.Position(-1));
            Assert.AreEqual(Tuple.Point(4.5, 3, 4), ray.Position(2.5));
        }

        [Test]
        public void Translating_a_ray()
        {
            var ray = new Ray(
                Tuple.Point(1, 2, 3),
                Tuple.Vector(0, 1, 0));
            var translation = Transformation.Translation(3, 4, 5);

            var ray2 = ray.Transform(translation);

            Assert.AreEqual(Tuple.Point(4, 6, 8), ray2.Origin);
            Assert.AreEqual(Tuple.Vector(0, 1, 0), ray2.Direction);
        }

        [Test]
        public void Scaling_a_ray()
        {
            var ray = new Ray(
                Tuple.Point(1, 2, 3),
                Tuple.Vector(0, 1, 0));
            var transformation = Transformation.Scaling(2, 3, 4);
            var ray2 = ray.Transform(transformation);

            Assert.AreEqual(Tuple.Point(2, 6, 12), ray2.Origin);
            Assert.AreEqual(Tuple.Vector(0, 3, 0), ray2.Direction);
        }
    }
}
