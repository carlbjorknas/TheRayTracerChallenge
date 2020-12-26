using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes;
using FluentAssertions;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Tests.Shapes
{
    [TestFixture]
    public class ConeTests
    {
        [Test]
        public void Intersecting_a_cone_with_a_ray()
        {
            Intersecting_a_cone_with_a_ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1), 5, 5);
            Intersecting_a_cone_with_a_ray(Tuple.Point(0, 0, -5), Tuple.Vector(1, 1, 1), 8.66025, 8.66025);
            Intersecting_a_cone_with_a_ray(Tuple.Point(1, 1, -5), Tuple.Vector(-0.5, -1, 1), 4.55006, 49.44994);
        }

        private void Intersecting_a_cone_with_a_ray(Tuple origin, Tuple direction, double t0, double t1)
        {
            var cone = new Cone();
            direction = direction.Normalize;
            var ray = new Ray(origin, direction);
            var xs = cone.LocalIntersect(ray);

            xs.Count.Should().Be(2);
            xs[0].T.Should().BeApproximately(t0, C.Epsilon);
            xs[1].T.Should().BeApproximately(t1, C.Epsilon);
        }

        [Test]
        public void Intersecting_a_cone_with_a_ray_parallel_to_one_of_its_halves()
        {
            var cone = new Cone();
            var direction = Tuple.Vector(0, 1, 1).Normalize;
            var ray = new Ray(Tuple.Point(0, 0, -1), direction);
            var xs = cone.LocalIntersect(ray);

            xs.Count.Should().Be(1);
            xs[0].T.Should().BeApproximately(0.35355, C.Epsilon);
        }
    }
}
