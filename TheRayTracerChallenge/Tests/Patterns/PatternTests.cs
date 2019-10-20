using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests.Patterns
{
    [TestFixture]
    class PatternTests
    {
        [Test]
        public void The_default_pattern_transformation()
        {
            var pattern = new TestPattern();
            Assert.AreEqual(Transformation.Identity, pattern.Transform);
        }

        [Test]
        public void Assigning_a_transformation()
        {
            var pattern = new TestPattern();
            pattern.Transform = Transformation.Translation(1, 2, 3);
            Assert.AreEqual(Transformation.Translation(1, 2, 3), pattern.Transform);
        }

        [Test]
        public void Stripes_with_an_object_transformation()
        {
            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Scaling(2, 2, 2);
            var pattern = new TestPattern();
            var color = pattern.PatternColorAtShape(sphere, Tuple.Point(2, 3, 4));

            Assert.AreEqual(new Color(1, 1.5, 2), color);
        }

        [Test]
        public void Stripes_with_a_pattern_transformation()
        {
            var sphere = Sphere.UnitSphere();
            var pattern = new TestPattern();
            pattern.Transform = Transformation.Scaling(2, 2, 2);
            var color = pattern.PatternColorAtShape(sphere, Tuple.Point(2, 3, 4));

            Assert.AreEqual(new Color(1, 1.5, 2), color);
        }

        [Test]
        public void Stripes_with_both_an_object_and_a_pattern_transformation()
        {
            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Scaling(2, 2, 2);
            var pattern = new TestPattern();
            pattern.Transform = Transformation.Translation(0.5, 1, 1.5);
            var color = pattern.PatternColorAtShape(sphere, Tuple.Point(2.5, 3, 3.5));

            Assert.AreEqual(new Color(0.75, 0.5, 0.25), color);
        }
    }
}
