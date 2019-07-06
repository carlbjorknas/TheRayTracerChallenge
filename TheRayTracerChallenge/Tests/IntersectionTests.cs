using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class IntersectionTests
    {
        [Test]
        public void An_intersection_encapsulates_t_and_object()
        {
            var sphere = Sphere.UnitSphere();
            var i = new Intersection(3.5, sphere);

            Assert.AreEqual(3.5, i.T);
            Assert.AreEqual(sphere, i.Object);
        }

        [Test]
        public void Aggregating_intersections()
        {
            var sphere = Sphere.UnitSphere();
            var i1 = new Intersection(1, sphere);
            var i2 = new Intersection(2, sphere);
            var intersections = new IntersectionCollection(i1, i2);
        }
    }
}
