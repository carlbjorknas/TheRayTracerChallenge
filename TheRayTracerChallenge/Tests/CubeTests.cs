using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class CubeTests
    {
        [Test]
        public void A_ray_intersects_a_cube()
        {
            A_ray_intersects_a_cube(Tuple.Point(  5, 0.5,  0), Tuple.Vector(-1,  0,  0),  4, 6); // +x
            A_ray_intersects_a_cube(Tuple.Point( -5, 0.5,  0), Tuple.Vector( 1,  0,  0),  4, 6); // -x
            A_ray_intersects_a_cube(Tuple.Point(0.5,   5,  0), Tuple.Vector( 0, -1,  0),  4, 6); // +y
            A_ray_intersects_a_cube(Tuple.Point(0.5,  -5,  0), Tuple.Vector( 0,  1,  0),  4, 6); // -y
            A_ray_intersects_a_cube(Tuple.Point(0.5,   0,  5), Tuple.Vector( 0,  0, -1),  4, 6); // +z
            A_ray_intersects_a_cube(Tuple.Point(0.5,   0, -5), Tuple.Vector( 0,  0,  1),  4, 6); // -z
            A_ray_intersects_a_cube(Tuple.Point(  0, 0.5,  0), Tuple.Vector( 0,  0,  1), -1, 1); // inside
        }

        private void A_ray_intersects_a_cube(Tuple origin, Tuple direction, double t1, double t2)
        {
            var c = new Cube();
            var ray = new Ray(origin, direction);
            var xs = c.LocalIntersect(ray);

            xs.Count.Should().Be(2);
            xs[0].T.Should().Be(t1);
            xs[1].T.Should().Be(t2);
        }

        [Test]
        public void A_ray_misses_a_cube()
        {
            A_ray_misses_a_cube(Tuple.Point(-2, 0, 0), Tuple.Vector(0.2673, 0.5345, 0.8018));
            A_ray_misses_a_cube(Tuple.Point(0, -2, 0), Tuple.Vector(0.8018, 0.2673, 0.5345));
            A_ray_misses_a_cube(Tuple.Point(0, 0, -2), Tuple.Vector(0.5345, 0.8018, 0.2673));
            A_ray_misses_a_cube(Tuple.Point(2, 0, 2), Tuple.Vector(0, 0, -1));
            A_ray_misses_a_cube(Tuple.Point(0, 2, 2), Tuple.Vector(0, -1, 0));
            A_ray_misses_a_cube(Tuple.Point(2, 2, 0), Tuple.Vector(-1, 0, 0));
        }

        private void A_ray_misses_a_cube(Tuple origin, Tuple direction)
        {
            var c = new Cube();
            var ray = new Ray(origin, direction);
            var xs = c.LocalIntersect(ray);

            xs.Count.Should().Be(0);
        }

        [Test]
        public void The_normal_on_the_surface_of_a_cube()
        {
            The_normal_on_the_surface_of_a_cube(Tuple.Point(1, 0.5, -0.8), Tuple.Vector(1, 0, 0));
            The_normal_on_the_surface_of_a_cube(Tuple.Point(-1, -0.2, 0.9), Tuple.Vector(-1, 0, 0));
            The_normal_on_the_surface_of_a_cube(Tuple.Point(-0.4, 1, -0.1), Tuple.Vector(0, 1, 0));
            The_normal_on_the_surface_of_a_cube(Tuple.Point(0.3, -1, -0.7), Tuple.Vector(0, -1, 0));
            The_normal_on_the_surface_of_a_cube(Tuple.Point(-0.6, 0.3, 1), Tuple.Vector(0, 0, 1));
            The_normal_on_the_surface_of_a_cube(Tuple.Point(0.4, 0.4, -1), Tuple.Vector(0, 0, -1));
            The_normal_on_the_surface_of_a_cube(Tuple.Point(1, 1, 1), Tuple.Vector(1, 0, 0));
            The_normal_on_the_surface_of_a_cube(Tuple.Point(-1, -1, -1), Tuple.Vector(-1, 0, 0));
        }

        private void The_normal_on_the_surface_of_a_cube(Tuple point, Tuple expectedNormal)
        {
            var c = new Cube();
            var normal = c.NormalAt(point);
            normal.Should().Be(normal);
        }
    }
}
