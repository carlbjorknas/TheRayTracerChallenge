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

        [Test]
        public void The_hit_when_all_intersections_have_positive_t()
        {
            var sphere = Sphere.UnitSphere();
            var i1 = new Intersection(1, sphere);
            var i2 = new Intersection(2, sphere);
            var intersections = new IntersectionCollection(i2, i1);

            var hit = intersections.Hit();

            Assert.AreEqual(i1, hit);
        }

        [Test]
        public void The_hit_when_some_intersections_have_negative_t()
        {
            var sphere = Sphere.UnitSphere();
            var i1 = new Intersection(-1, sphere);
            var i2 = new Intersection(1, sphere);
            var intersections = new IntersectionCollection(i2, i1);

            var hit = intersections.Hit();

            Assert.AreEqual(i2, hit);
        }

        [Test]
        public void The_hit_when_all_intersections_have_negative_t()
        {
            var sphere = Sphere.UnitSphere();
            var i1 = new Intersection(-2, sphere);
            var i2 = new Intersection(-1, sphere);
            var intersections = new IntersectionCollection(i2, i1);

            var hit = intersections.Hit();

            Assert.AreEqual(null, hit);
        }

        [Test]
        public void The_hit_is_always_the_lowest_nonnegative_intersection()
        {
            var sphere = Sphere.UnitSphere();
            var i1 = new Intersection(5, sphere);
            var i2 = new Intersection(7, sphere);
            var i3 = new Intersection(-3, sphere);
            var i4 = new Intersection(2, sphere);
            var intersections = new IntersectionCollection(i1, i2, i3, i4);

            var hit = intersections.Hit();

            Assert.AreEqual(i4, hit);
        }
    }
}
