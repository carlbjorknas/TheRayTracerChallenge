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
            var ray = new Ray(
                Tuple.Point(0, 1, -5),
                Tuple.Vector(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(2, intersections.Count);
            Assert.AreEqual(5.0, intersections[0].T);
            Assert.AreEqual(5.0, intersections[1].T);
        }

        [Test]
        public void A_ray_misses_a_sphere()
        {
            var ray = new Ray(
                Tuple.Point(0, 2, -5),
                Tuple.Vector(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(0, intersections.Count);
        }

        [Test]
        public void A_ray_originates_inside_a_sphere()
        {
            var ray = new Ray(
                Tuple.Point(0, 0, 0),
                Tuple.Vector(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(2, intersections.Count);
            Assert.AreEqual(-1.0, intersections[0].T);
            Assert.AreEqual(1.0, intersections[1].T);
        }

        [Test]
        public void A_sphere_is_behind_a_ray()
        {
            var ray = new Ray(
                Tuple.Point(0, 0, 5),
                Tuple.Vector(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(2, intersections.Count);
            Assert.AreEqual(-6.0, intersections[0].T);
            Assert.AreEqual(-4.0, intersections[1].T);
        }    
        
        [Test]
        public void Intersect_sets_the_object_on_the_intersection()
        {
            var ray = new Ray(
                Tuple.Point(0, 0, -5),
                Tuple.Vector(0, 0, 1));

            var sphere = Sphere.UnitSphere();

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(2, intersections.Count);
            Assert.AreEqual(sphere, intersections[0].Object);
            Assert.AreEqual(sphere, intersections[1].Object);
        }

        [Test]
        public void A_spheres_default_transformation()
        {
            var s = Sphere.UnitSphere();
            Assert.AreSame(Transformation.Identity, s.Transform);
        }

        [Test]
        public void Intersecting_a_scaled_sphere_with_a_ray()
        {
            var ray = new Ray(
                Tuple.Point(0, 0, -5),
                Tuple.Vector(0, 0, 1));
            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Scaling(2,2,2);

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(2, intersections.Count);
            Assert.AreEqual(3, intersections[0].T);
            Assert.AreEqual(7, intersections[1].T);
        }

        [Test]
        public void Intersecting_a_translated_sphere_with_a_ray()
        {
            var ray = new Ray(
                Tuple.Point(0, 0, -5),
                Tuple.Vector(0, 0, 1));
            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Translation(5, 0, 0);

            var intersections = sphere.Intersect(ray);

            Assert.AreEqual(0, intersections.Count);
        }

        [Test]
        public void The_normal_on_a_sphere_at_a_point_on_the_x_axis()
        {
            var sphere = Sphere.UnitSphere();
            var normal = sphere.NormalAt(Tuple.Point(1, 0, 0));
            Assert.AreEqual(Tuple.Vector(1, 0, 0), normal);
        }

        [Test]
        public void The_normal_on_a_sphere_at_a_point_on_the_y_axis()
        {
            var sphere = Sphere.UnitSphere();
            var normal = sphere.NormalAt(Tuple.Point(0, 1, 0));
            Assert.AreEqual(Tuple.Vector(0, 1, 0), normal);
        }

        [Test]
        public void The_normal_on_a_sphere_at_a_point_on_the_z_axis()
        {
            var sphere = Sphere.UnitSphere();
            var normal = sphere.NormalAt(Tuple.Point(0, 0, 1));
            Assert.AreEqual(Tuple.Vector(0, 0, 1), normal);
        }

        [Test]
        public void The_normal_on_a_sphere_at_a_nonaxial_point()
        {
            var sphere = Sphere.UnitSphere();
            var p = Math.Sqrt(3) / 3;
            var normal = sphere.NormalAt(Tuple.Point(p, p, p));
            Assert.AreEqual(Tuple.Vector(p, p, p), normal);
        }

        [Test]
        public void The_normal_is_a_normalized_vector()
        {
            var sphere = Sphere.UnitSphere();
            var p = Math.Sqrt(3) / 3;
            var normal = sphere.NormalAt(Tuple.Point(p, p, p));
            Assert.AreEqual(normal, normal.Normalize);
        }

        [Test]
        public void Computing_the_normal_on_a_translated_sphere()
        {
            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Translation(0, 1, 0);

            var n = sphere.NormalAt(Tuple.Point(0, 1.70711, -0.70711));

            Assert.AreEqual(Tuple.Vector(0, 0.70711, -0.70711), n);
        }

        [Test]
        public void Computing_the_normal_on_a_transformed_sphere()
        {
            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Scaling(1, 0.5, 1)
                .Chain(Transformation.RotationZ(Math.PI / 5));

            var n = sphere.NormalAt(Tuple.Point(0, Math.Sqrt(2)/2, -Math.Sqrt(2) / 2));

            Assert.AreEqual(Tuple.Vector(0, 0.97014, -0.24254), n);
        }

        [Test]
        public void A_sphere_has_a_default_material()
        {
            var s = Sphere.UnitSphere();
            Assert.AreEqual(new Material(), s.Material);
        }

        [Test]
        public void A_sphere_may_be_assigned_a_material()
        {
            var s = Sphere.UnitSphere();
            var m = new Material();

            s.Material = m;

            Assert.AreEqual(m, s.Material);
        }
    }
}
