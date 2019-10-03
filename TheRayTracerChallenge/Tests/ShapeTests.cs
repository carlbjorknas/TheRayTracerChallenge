using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    class ShapeTests
    {
        [Test]
        public void The_default_transformation()
        {
            var shape = new TestShape();
            Assert.AreSame(Transformation.Identity, shape.Transform);
        }

        [Test]
        public void Assigning_a_transformation()
        {
            var shape = new TestShape();
            shape.Transform = Transformation.Translation(2, 3, 4);
            Assert.AreEqual(Transformation.Translation(2,3,4), shape.Transform);
        }

        [Test]
        public void The_default_material()
        {
            var s = new TestShape();
            Assert.AreEqual(new Material(), s.Material);
        }

        [Test]
        public void Assigning_a_material()
        {
            var s = new TestShape();
            var m = new Material();

            s.Material = m;

            Assert.AreEqual(m, s.Material);
        }

        [Test]
        public void Intersecting_a_scaled_shaped_with_a_ray()
        {
            var ray = new Ray(
                Tuple.Point(0, 0, -5),
                Tuple.Vector(0, 0, 1));
            var shape = new TestShape();
            shape.Transform = Transformation.Scaling(2, 2, 2);

            shape.Intersect(ray);

            Assert.AreEqual(Tuple.Point(0,0,-2.5), shape.LocalRay.Point);
            Assert.AreEqual(Tuple.Vector(0,0,0.5), shape.LocalRay.Direction);
        }

        [Test]
        public void Intersecting_a_translated_shape_with_a_ray()
        {
            var ray = new Ray(
                Tuple.Point(0, 0, -5),
                Tuple.Vector(0, 0, 1));
            var shape = new TestShape();
            shape.Transform = Transformation.Translation(5, 0, 0);

            shape.Intersect(ray);

            Assert.AreEqual(Tuple.Point(-5, 0, -5), shape.LocalRay.Point);
            Assert.AreEqual(Tuple.Vector(0, 0, 1), shape.LocalRay.Direction);
        }
    }
}
