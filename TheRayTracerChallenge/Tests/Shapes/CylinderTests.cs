using NUnit.Framework;
using FluentAssertions;
using TheRayTracerChallenge.Shapes;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Tests.Shapes
{
    [TestFixture]
    public class CylinderTests
    {
        [Test]
        public void A_ray_misses_a_cylinder()
        {
            A_ray_misses_a_cylinder(Tuple.Point(1, 0, 0), Tuple.Vector(0, 1, 0));
            A_ray_misses_a_cylinder(Tuple.Point(0, 0, 0), Tuple.Vector(0, 1, 0));
            A_ray_misses_a_cylinder(Tuple.Point(0, 0, -5), Tuple.Vector(1, 1, 1));
        }

        private void A_ray_misses_a_cylinder(Tuple origin, Tuple direction)
        {
            var cylinder = new Cylinder();
            direction = direction.Normalize;
            var ray = new Ray(origin, direction);
            var xs = cylinder.Intersect(ray);
            xs.Count.Should().Be(0);
        }

        [Test]
        public void A_ray_strikes_a_cylinder()
        {
            A_ray_strikes_a_cylinder(Tuple.Point(1, 0, -5), Tuple.Vector(0, 0, 1), 5, 5);
            A_ray_strikes_a_cylinder(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1), 4, 6);
            A_ray_strikes_a_cylinder(Tuple.Point(0.5, 0, -5), Tuple.Vector(0.1, 1, 1), 6.80798, 7.08872);
        }

        private void A_ray_strikes_a_cylinder(Tuple origin, Tuple direction, double t0, double t1)
        {
            var cylinder = new Cylinder();
            direction = direction.Normalize;
            var ray = new Ray(origin, direction);
            var xs = cylinder.Intersect(ray);

            xs.Count.Should().Be(2);
            xs[0].T.Should().BeApproximately(t0, C.Epsilon);
            xs[1].T.Should().BeApproximately(t1, C.Epsilon);
        }
    }
}
