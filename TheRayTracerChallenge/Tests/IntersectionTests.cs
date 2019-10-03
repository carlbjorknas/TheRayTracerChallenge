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

        [Test]
        public void Precomputing_the_state_of_an_intersection()
        {
            var ray = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            var shape = Sphere.UnitSphere();
            var i = new Intersection(4, shape);

            var comps = i.PrepareComputations(ray);

            Assert.AreEqual(i.T, comps.T);
            Assert.AreEqual(i.Object, comps.Object);
            Assert.AreEqual(Tuple.Point(0, 0, -1), comps.Point);
            Assert.AreEqual(Tuple.Vector(0, 0, -1), comps.EyeVector);
            Assert.AreEqual(Tuple.Vector(0, 0, -1), comps.NormalVector);
        }

        [Test]
        public void The_hit_when_an_intersection_occurs_on_the_outside()
        {
            var ray = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            var shape = Sphere.UnitSphere();
            var i = new Intersection(4, shape);

            var comps = i.PrepareComputations(ray);

            Assert.IsFalse(comps.Inside);
        }

        [Test]
        public void The_hit_when_an_intersection_occurs_on_the_inside()
        {
            var ray = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            var shape = Sphere.UnitSphere();
            var i = new Intersection(1, shape);

            var comps = i.PrepareComputations(ray);

            Assert.AreEqual(Tuple.Point(0, 0, 1), comps.Point);
            Assert.AreEqual(Tuple.Vector(0, 0, -1), comps.EyeVector);
            Assert.IsTrue(comps.Inside);
            Assert.AreEqual(Tuple.Vector(0, 0, -1), comps.NormalVector);
        }

        [Test]
        public void The_hit_should_offset_the_point()
        {
            var ray = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Translation(0, 0, 1);
            var intersection = new Intersection(5, sphere);

            var comps = intersection.PrepareComputations(ray);

            Assert.Less(comps.OverPoint.z, -Constants.Epsilon / 2);
            Assert.Greater(comps.Point.z, comps.OverPoint.z);
        }
    }
}
