using MathNet.Numerics;
using NUnit.Framework;
using System;
using TheRayTracerChallenge.Shapes;
using TheRayTracerChallenge.Utils;

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

            Assert.Less(comps.OverPoint.z, -C.Epsilon / 2);
            Assert.Greater(comps.Point.z, comps.OverPoint.z);
        }

        [Test]
        public void Precomputing_the_reflection_vector()
        {
            var plane = new Plane();
            var ray = new Ray(Tuple.Point(0, 1, -1), Tuple.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var i = new Intersection(Math.Sqrt(2), plane);
            var comps = i.PrepareComputations(ray);
            Assert.AreEqual(Tuple.Vector(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2), comps.ReflectV);
        }

        // Test setup as in the image "Refractive_test_setup" included in the project.
        [Test]
        [TestCase(0, 1.0, 1.5)]
        [TestCase(1, 1.5, 2.0)]
        [TestCase(2, 2.0, 2.5)]
        [TestCase(3, 2.5, 2.5)]
        [TestCase(4, 2.5, 1.5)]
        [TestCase(5, 1.5, 1.0)]
        public void Finding_n1_and_n2_at_various_intersections(int index, double n1, double n2)
        {
            var a = Sphere.Glass();
            a.Transform = Transformation.Scaling(2, 2, 2);
            a.Material.RefractiveIndex = 1.5;

            var b = Sphere.Glass();
            b.Transform = Transformation.Translation(0, 0, -0.25);
            b.Material.RefractiveIndex = 2.0;

            var c = Sphere.Glass();
            c.Transform = Transformation.Translation(0, 0, 0.25);
            c.Material.RefractiveIndex = 2.5;

            var ray = new Ray(Tuple.Point(0, 0, -4), Tuple.Vector(0, 0, 1));
            var xs = new IntersectionCollection(
                new Intersection(2, a),
                new Intersection(2.75, b),
                new Intersection(3.25, c),
                new Intersection(4.75, b),
                new Intersection(5.25, c),
                new Intersection(6, a));

            var comps = xs[index].PrepareComputations(ray, xs);

            Assert.AreEqual(n1, comps.n1);
            Assert.AreEqual(n2, comps.n2);
        }

        [Test]
        public void The_under_point_is_offset_below_the_surface()
        {
            var r = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            var shape = Sphere.Glass();
            shape.Transform = Transformation.Translation(0, 0, 1);
            var i = new Intersection(5, shape);
            var xs = new IntersectionCollection(i);

            var comps = i.PrepareComputations(r, xs);
            Assert.Greater(comps.UnderPoint.z, C.Epsilon / 2);
            Assert.Less(comps.Point.z, comps.UnderPoint.z);
        }

        /*
         * Position a ray inside a glass sphere, offset from the center and pointing straight
         * up. The ray is offset sufficiently to trigger total internal reflection, resulting
         * in schlick() returning 1.
         */
        [Test]
        public void The_Schlick_approximation_under_total_internal_reflection()
        {
            var shape = Sphere.Glass();
            var ray = new Ray(Tuple.Point(0, 0, C.SqrtOf2DividedBy2), Tuple.Vector(0, 1, 0));
            var xs = new IntersectionCollection(
                new Intersection(-C.SqrtOf2DividedBy2, shape),
                new Intersection(C.SqrtOf2DividedBy2, shape));
            var comps = xs[1].PrepareComputations(ray, xs);

            var reflectance = comps.Schlick();

            Assert.AreEqual(1.0, reflectance);
        }

        /*
         * Create a glass sphere and a ray that intersects it. The ray should strike the
         * sphere perpendicular to its surface. The reflectance in this case will be slight.
         */
        [Test]
        public void The_Schlick_approximation_with_a_perpendicular_viewing_angle()
        {
            var shape = Sphere.Glass();
            var ray = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 1, 0));
            var xs = new IntersectionCollection(
                new Intersection(-1, shape),
                new Intersection(1, shape));
            var comps = xs[1].PrepareComputations(ray, xs);

            var reflectance = comps.Schlick();

            Assert.AreEqual(0.04, reflectance, C.Epsilon);
        }

        /*
         * This is the “looking across the lake to the far shore” scenario, and a significant
         * amount of light should be reflected. The test mimics this by preparing a ray
         * so that it glances off a sphere, almost tangent to it.
         */
        [Test]
        public void The_Schlick_approximation_with_small_angle_and_n2_greater_than_n1()
        {
            var shape = Sphere.Glass();
            var ray = new Ray(Tuple.Point(0, 0.99, -2), Tuple.Vector(0, 0, 1));
            var xs = new IntersectionCollection(
                new Intersection(1.8589, shape));
            var comps = xs[0].PrepareComputations(ray, xs);

            var reflectance = comps.Schlick();

            Assert.AreEqual(0.48873, reflectance, C.Epsilon);
        }

        [Test]
        public void An_intersection_can_encapsulate_u_and_v()
        {
            var triangle = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            var i = new Intersection(3.5, triangle, 0.2, 0.4);
            Assert.AreEqual(0.2, i.U);
            Assert.AreEqual(0.4, i.V);
        }
    }
}
