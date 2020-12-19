using NUnit.Framework;
using FluentAssertions;
using TheRayTracerChallenge.Shapes;

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
    }
}
