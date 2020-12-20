using NUnit.Framework;
using FluentAssertions;
using TheRayTracerChallenge.Shapes;
using TheRayTracerChallenge.Utils;
using System;

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

        [Test]
        public void Normal_vector_on_a_cylinder()
        {
            Normal_vector_on_a_cylinder(Tuple.Point(1, 0, 0), Tuple.Vector(1, 0, 0));
            Normal_vector_on_a_cylinder(Tuple.Point(0, 5, -1), Tuple.Vector(0, 0, -1));
            Normal_vector_on_a_cylinder(Tuple.Point(0, -2, 1), Tuple.Vector(0, 0, 1));
            Normal_vector_on_a_cylinder(Tuple.Point(-1, 1, 0), Tuple.Vector(-1, 0, 0));
        }

        private void Normal_vector_on_a_cylinder(Tuple point, Tuple expectedNormal)
        {
            var cylinder = new Cylinder();
            var normal = cylinder.LocalNormalAt(point);
            normal.Should().Be(expectedNormal);
        }

        [Test]
        public void The_default_minimum_and_maximum_for_a_cylinder()
        {
            var cylinder = new Cylinder();
            cylinder.Min.Should().Be(double.NegativeInfinity);
            cylinder.Max.Should().Be(double.PositiveInfinity);
        }

        [Test]
        public void Intersecting_a_constrained_cylinder()
        {
            // From the inside, diagonally upwards, won't intersect the cylinder
            Intersecting_a_constrained_cylinder(Tuple.Point(0, 1.5, 0), Tuple.Vector(0.1, 1, 0), 0);

            // Perpendicular to the y-axis, but first too high and the second too low
            Intersecting_a_constrained_cylinder(Tuple.Point(0, 3, -5), Tuple.Vector(0, 0, 1), 0);
            Intersecting_a_constrained_cylinder(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1), 0);

            // Edge cases, shows that min and max is not included in the cylinder
            Intersecting_a_constrained_cylinder(Tuple.Point(0, 2, -5), Tuple.Vector(0, 0, 1), 0);
            Intersecting_a_constrained_cylinder(Tuple.Point(0, 1, -5), Tuple.Vector(0, 0, 1), 0);

            // A ray through the cylinder, perpendicular to y-axis
            Intersecting_a_constrained_cylinder(Tuple.Point(0, 1.5, -2), Tuple.Vector(0, 0, 1), 2);
        }

        private void Intersecting_a_constrained_cylinder(Tuple origin, Tuple direction, int count)
        {
            var cylinder = new Cylinder(1, 2);
            direction = direction.Normalize;
            var ray = new Ray(origin, direction);
            var xs = cylinder.LocalIntersect(ray);
            xs.Count.Should().Be(count);
        }

        [Test]
        public void The_default_closed_value_for_a_cylinder()
        {
            var cylinder = new Cylinder();
            cylinder.Closed.Should().BeFalse();
        }

        [Test]
        public void Intersecting_the_caps_of_a_closed_cylinder()
        {
            Intersecting_the_caps_of_a_closed_cylinder(Tuple.Point(0, 3, 0), Tuple.Vector(0, -1, 0), 2);
            Intersecting_the_caps_of_a_closed_cylinder(Tuple.Point(0, 3, -2), Tuple.Vector(0, -1, 2), 2);
            Intersecting_the_caps_of_a_closed_cylinder(Tuple.Point(0, 4, -2), Tuple.Vector(0, -1, 1), 2); // Corner case
            Intersecting_the_caps_of_a_closed_cylinder(Tuple.Point(0, 0, -2), Tuple.Vector(0, 1, 2), 2);
            Intersecting_the_caps_of_a_closed_cylinder(Tuple.Point(0, -1, -2), Tuple.Vector(0, 1, 1), 2); // Corner case
        }

        private void Intersecting_the_caps_of_a_closed_cylinder(Tuple origin, Tuple direction, int count)
        {
            var cylinder = new Cylinder(1, 2, closed: true);
            direction = direction.Normalize;
            var ray = new Ray(origin, direction);
            var xs = cylinder.LocalIntersect(ray);
            xs.Count.Should().Be(count);
        }

        [Test]
        public void The_normal_vector_on_a_cylinders_end_caps()
        {
            The_normal_vector_on_a_cylinders_end_caps(Tuple.Point(0, 1, 0), Tuple.Vector(0, -1, 0));
            The_normal_vector_on_a_cylinders_end_caps(Tuple.Point(0.5, 1, 0), Tuple.Vector(0, -1, 0));
            The_normal_vector_on_a_cylinders_end_caps(Tuple.Point(0, 1, 0.5), Tuple.Vector(0, -1, 0));
            The_normal_vector_on_a_cylinders_end_caps(Tuple.Point(0, 2, 0), Tuple.Vector(0, 1, 0));
            The_normal_vector_on_a_cylinders_end_caps(Tuple.Point(0.5, 2, 0), Tuple.Vector(0, 1, 0));
            The_normal_vector_on_a_cylinders_end_caps(Tuple.Point(0, 2, 0.5), Tuple.Vector(0, 1, 0));
        }

        private void The_normal_vector_on_a_cylinders_end_caps(Tuple point, Tuple expectedNormal)
        {
            var cylinder = new Cylinder(1, 2, closed: true);
            var normal = cylinder.LocalNormalAt(point);
            normal.Should().Be(expectedNormal);
        }
    }
}
