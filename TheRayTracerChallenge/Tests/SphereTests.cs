using MathNet.Spatial.Euclidean;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class SphereTests
    {
        [Test]
        public void A_ray_intersects_a_sphere_at_a_tangent()
        {
            var ray = new Ray3D(
                new Point3D(0, 1, -5),
                new Vector3D(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(2, intersections.Count);
            Assert.AreEqual(5.0, intersections[0].T);
            Assert.AreEqual(5.0, intersections[1].T);
        }

        [Test]
        public void A_ray_misses_a_sphere()
        {
            var ray = new Ray3D(
                new Point3D(0, 2, -5),
                new Vector3D(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(0, intersections.Count);
        }

        [Test]
        public void A_ray_originates_inside_a_sphere()
        {
            var ray = new Ray3D(
                new Point3D(0, 0, 0),
                new Vector3D(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(2, intersections.Count);
            Assert.AreEqual(-1.0, intersections[0].T);
            Assert.AreEqual(1.0, intersections[1].T);
        }

        [Test]
        public void A_sphere_is_behind_a_ray()
        {
            var ray = new Ray3D(
                new Point3D(0, 0, 5),
                new Vector3D(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(2, intersections.Count);
            Assert.AreEqual(-6.0, intersections[0].T);
            Assert.AreEqual(-4.0, intersections[1].T);
        }    
        
        [Test]
        public void Intersect_sets_the_object_on_the_intersection()
        {
            var ray = new Ray3D(
                new Point3D(0, 0, -5),
                new Vector3D(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(2, intersections.Count);
            Assert.AreEqual(sphere, intersections[0].Object);
            Assert.AreEqual(sphere, intersections[1].Object);
        }
    }
}
