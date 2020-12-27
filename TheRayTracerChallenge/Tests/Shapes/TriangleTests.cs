using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.Tests.Shapes
{
    [TestFixture]
    public class TriangleTests
    {
        [Test]
        public void Constructing_a_triangle()
        {
            var p1 = Tuple.Point(0, 1, 0);
            var p2 = Tuple.Point(-1, 0, 0);
            var p3 = Tuple.Point(1, 0, 0);
            var triangle = new Triangle(p1, p2, p3);
            triangle.P1.Should().Be(p1);
            triangle.P2.Should().Be(p2);
            triangle.P3.Should().Be(p3);
            triangle.EdgeVec1.Should().Be(Tuple.Vector(-1, -1, 0));
            triangle.EdgeVec2.Should().Be(Tuple.Vector(1, -1, 0));
            triangle.Normal.Should().Be(Tuple.Vector(0, 0, -1));
        }

        [Test]
        public void Finding_the_normal_on_a_triangle()
        {
            var triangle = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            var n1 = triangle.LocalNormalAt(Tuple.Point(0, 0.5, 0));
            var n2 = triangle.LocalNormalAt(Tuple.Point(-0.5, 0.75, 0));
            var n3 = triangle.LocalNormalAt(Tuple.Point(0.5, 0.25, 0));
            n1.Should().Be(triangle.Normal);
            n2.Should().Be(triangle.Normal);
            n3.Should().Be(triangle.Normal);
        }

        [Test]
        public void Intersecting_a_ray_parallel_to_the_triangle()
        {
            var triangle = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            var ray = new Ray(Tuple.Point(0, -1, -2), Tuple.Vector(0, 1, 0));
            var xs = triangle.LocalIntersect(ray);
            xs.Count.Should().Be(0);
        }

        [Test]
        public void A_ray_misses_the_P1_to_P3_edge()
        {
            var triangle = new Triangle(Tuple.Point(0, 1, 0), Tuple.Point(-1, 0, 0), Tuple.Point(1, 0, 0));
            var ray = new Ray(Tuple.Point(1, 1, -2), Tuple.Vector(0, 0, 1));
            var xs = triangle.LocalIntersect(ray);
            xs.Count.Should().Be(0);
        }
    }
}
