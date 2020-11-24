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

        private void A_ray_intersects_a_cube(Tuple rayPoint, Tuple rayDirection, double t1, double t2)
        {
            var c = new Cube();
            var ray = new Ray(rayPoint, rayDirection);
            var xs = c.LocalIntersect(ray);

            xs.Count.Should().Be(2);
            xs[0].T.Should().Be(t1);
            xs[1].T.Should().Be(t2);
        }
    }
}
