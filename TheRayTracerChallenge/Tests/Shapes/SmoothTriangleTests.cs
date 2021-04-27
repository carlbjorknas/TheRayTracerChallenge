using FluentAssertions;
using MathNet.Numerics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Tests.Shapes
{
    [TestFixture]
    public class SmoothTriangleTests
    {
        private Tuple _p1;
        private Tuple _p2;
        private Tuple _p3;
        private Tuple _n1;
        private Tuple _n2;
        private Tuple _n3;
        private SmoothTriangle _tri;

        [SetUp]
        public void Init()
        {
            _p1 = Tuple.Point(0, 1, 0);
            _p2 = Tuple.Point(-1, 0, 0);
            _p3 = Tuple.Point(1, 0, 0);
            _n1 = Tuple.Vector(0, 1, 0);
            _n2 = Tuple.Vector(-1, 0, 0);
            _n3 = Tuple.Vector(1, 0, 0);
            _tri = new SmoothTriangle(_p1, _p2, _p3, _n1, _n2, _n3);
        }

        [Test]
        public void Constructing_a_smooth_triangle()
        {
            Assert.AreEqual(_p1, _tri.P1);
            Assert.AreEqual(_p2, _tri.P2);
            Assert.AreEqual(_p3, _tri.P3);
            Assert.AreEqual(_n1, _tri.N1);
            Assert.AreEqual(_n2, _tri.N2);
            Assert.AreEqual(_n3, _tri.N3);
        }

        [Test]
        public void An_intersection_with_a_smooth_triangle_stores_u_and_v()
        {
            var ray = new Ray(Tuple.Point(-0.2, 0.3, -2), Tuple.Vector(0, 0, 1));
            var xs = _tri.LocalIntersect(ray);
            Assert.AreEqual(1, xs.Count);
            xs[0].U.Should().BeApproximately(0.45, C.Epsilon);
            xs[0].V.Should().BeApproximately(0.25, C.Epsilon);
        }

        [Test]
        public void A_smooth_triangle_uses_u_and_v_to_interpolate_the_normal()
        {
            var i = new Intersection(1, _tri, 0.45, 0.25);
            var n = _tri.NormalAt(Tuple.Point(0, 0, 0), i);
            n.Should().Be(Tuple.Vector(-0.5547, 0.83205, 0));
        }

        [Test]
        public void Preparing_the_normal_on_a_smooth_triangle()
        {
            var i = new Intersection(1, _tri, 0.45, 0.25);
            var ray = new Ray(Tuple.Point(-0.2, 0.3, -2), Tuple.Vector(0, 0, 1));
            var xs = new IntersectionCollection(i);
            var comps = i.PrepareComputations(ray, xs);
            comps.NormalVector.Should().Be(Tuple.Vector(-0.5547, 0.83205, 0));
        }
    }
}
