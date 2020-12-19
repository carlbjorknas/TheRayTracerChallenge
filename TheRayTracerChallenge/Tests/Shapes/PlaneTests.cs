using NUnit.Framework;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.Tests.Shapes
{
    [TestFixture]
    class PlaneTests
    {
        [Test]
        public void The_normal_of_a_plane_is_constant_everywhere()
        {
            var plane = new Plane();
            var n1 = plane.LocalNormalAt(Tuple.Point(0, 0, 0));
            var n2 = plane.LocalNormalAt(Tuple.Point(10, 0, -10));
            var n3 = plane.LocalNormalAt(Tuple.Point(-5, 0, 150));

            Assert.AreEqual(Tuple.Vector(0, 1, 0), n1);
            Assert.AreEqual(Tuple.Vector(0, 1, 0), n2);
            Assert.AreEqual(Tuple.Vector(0, 1, 0), n3);
        }

        [Test]
        public void Intersect_with_a_ray_parallel_to_the_plane()
        {
            var plane = new Plane();
            var ray = new Ray(Tuple.Point(0, 10, 0), Tuple.Vector(0, 0, 1));
            var intersections = plane.LocalIntersect(ray);

            Assert.AreEqual(0, intersections.Count);
        }

        [Test]
        public void Intersect_with_a_coplanar_ray()
        {
            var plane = new Plane();
            var ray = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            var intersections = plane.LocalIntersect(ray);

            Assert.AreEqual(0, intersections.Count);
        }

        [Test]
        public void A_ray_intersecting_a_plane_from_above()
        {
            var plane = new Plane();
            var ray = new Ray(Tuple.Point(0, 1, 0), Tuple.Vector(0, -1, 0));
            var intersections = plane.LocalIntersect(ray);

            Assert.AreEqual(1, intersections.Count);
            Assert.AreEqual(1, intersections[0].T);
            Assert.AreEqual(plane, intersections[0].Object);
        }

        [Test]
        public void A_ray_intersecting_a_plane_from_below()
        {
            var plane = new Plane();
            var ray = new Ray(Tuple.Point(0, -1, 0), Tuple.Vector(0, 1, 0));
            var intersections = plane.LocalIntersect(ray);

            Assert.AreEqual(1, intersections.Count);
            Assert.AreEqual(1, intersections[0].T);
            Assert.AreEqual(plane, intersections[0].Object);
        }
    }
}
