using MathNet.Spatial.Euclidean;
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
            var ray = new Ray3D(
                new Point3D(2, 3, 4), 
                new Vector3D(1, 0, 0));

            Assert.AreEqual(new Point3D(2, 3, 4), ray.Position(0));
            Assert.AreEqual(new Point3D(3, 3, 4), ray.Position(1));
            Assert.AreEqual(new Point3D(1, 3, 4), ray.Position(-1));
            Assert.AreEqual(new Point3D(4.5, 3, 4), ray.Position(2.5));
        }

        [Test]
        public void A_ray_intersects_a_sphere_at_a_tangent()
        {
            var ray = new Ray3D(
                new Point3D(0, 1, -5),
                new Vector3D(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = ray.Intersect(sphere);

            Assert.AreEqual(2, intersections.Length);
            Assert.AreEqual(5.0, intersections[0]);
            Assert.AreEqual(5.0, intersections[1]);
        }

        [Test]
        public void A_ray_misses_a_sphere()
        {
            var ray = new Ray3D(
                new Point3D(0, 2, -5),
                new Vector3D(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = ray.Intersect(sphere);

            Assert.AreEqual(0, intersections.Length);
        }

        [Test]
        public void A_ray_originates_inside_a_sphere()
        {
            var ray = new Ray3D(
                new Point3D(0, 0, 0),
                new Vector3D(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = ray.Intersect(sphere);

            Assert.AreEqual(2, intersections.Length);
            Assert.AreEqual(-1.0, intersections[0]);
            Assert.AreEqual(1.0, intersections[1]);
        }

        [Test]
        public void A_sphere_is_behind_a_ray()
        {
            var ray = new Ray3D(
                new Point3D(0, 0, 5),
                new Vector3D(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = ray.Intersect(sphere);

            Assert.AreEqual(2, intersections.Length);
            Assert.AreEqual(-6.0, intersections[0]);
            Assert.AreEqual(-4.0, intersections[1]);
        }
    }
}
