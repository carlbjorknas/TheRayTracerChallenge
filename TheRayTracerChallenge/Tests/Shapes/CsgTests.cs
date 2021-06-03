using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.Tests.Shapes
{
    [TestFixture]
    public class CsgTests
    {
        [Test]
        public void CSG_is_created_with_an_operation_and_two_shapes()
        {
            var s1 = Sphere.UnitSphere();
            var s2 = new Cube();
            
            var csg = new Csg(CsgOperation.Union, s1, s2);

            csg.Left.Should().Be(s1);
            csg.Right.Should().Be(s2);
            s1.Parent.Should().Be(csg);
            s2.Parent.Should().Be(csg);
        }

        [TestCase(CsgOperation.Union, CsgOperand.Left, true, true, false)]
        [TestCase(CsgOperation.Union, CsgOperand.Left, true, false, true)]
        [TestCase(CsgOperation.Union, CsgOperand.Left, false, true, false)]
        [TestCase(CsgOperation.Union, CsgOperand.Left, false, false, true)]
        [TestCase(CsgOperation.Union, CsgOperand.Right, true, true, false)]
        [TestCase(CsgOperation.Union, CsgOperand.Right, true, false, false)]
        [TestCase(CsgOperation.Union, CsgOperand.Right, false, true, true)]
        [TestCase(CsgOperation.Union, CsgOperand.Right, false, false, true)]
        [TestCase(CsgOperation.Intersection, CsgOperand.Left, true, true, true)]
        [TestCase(CsgOperation.Intersection, CsgOperand.Left, true, false, false)]
        [TestCase(CsgOperation.Intersection, CsgOperand.Left, false, true, true)]
        [TestCase(CsgOperation.Intersection, CsgOperand.Left, false, false, false)]
        [TestCase(CsgOperation.Intersection, CsgOperand.Right, true, true, true)]
        [TestCase(CsgOperation.Intersection, CsgOperand.Right, true, false, true)]
        [TestCase(CsgOperation.Intersection, CsgOperand.Right, false, true, false)]
        [TestCase(CsgOperation.Intersection, CsgOperand.Right, false, false, false)]
        [TestCase(CsgOperation.Difference, CsgOperand.Left, true, true, false)]
        [TestCase(CsgOperation.Difference, CsgOperand.Left, true, false, true)]
        [TestCase(CsgOperation.Difference, CsgOperand.Left, false, true, false)]
        [TestCase(CsgOperation.Difference, CsgOperand.Left, false, false, true)]
        [TestCase(CsgOperation.Difference, CsgOperand.Right, true, true, true)]
        [TestCase(CsgOperation.Difference, CsgOperand.Right, true, false, true)]
        [TestCase(CsgOperation.Difference, CsgOperand.Right, false, true, false)]
        [TestCase(CsgOperation.Difference, CsgOperand.Right, false, false, false)]
        public void Evaluating_the_rule_for_a_CSG_operation(
            CsgOperation op, CsgOperand hitShape, bool insideLeft, bool insideRight, bool expectedResult)
        {
            var result = Csg.IntersectionAllowed(op, hitShape, insideLeft, insideRight);
            result.Should().Be(expectedResult);
        }

        [TestCase(CsgOperation.Union, 0, 3)]
        [TestCase(CsgOperation.Intersection, 1, 2)]
        [TestCase(CsgOperation.Difference, 0, 1)]
        public void Filtering_a_list_of_intersections(CsgOperation operation, int index1, int index2)
        {
            var s1 = Sphere.UnitSphere();
            var s2 = new Cube();
            var csg = new Csg(operation, s1, s2);
            var xs = IntersectionCollection.Create((1, s1), (2, s2), (3, s1), (4, s2));
            var result = csg.FilterIntersections(xs);
            result.Intersections.Should().HaveCount(2);
            result[0].Should().Be(xs[index1]);
            result[1].Should().Be(xs[index2]);
        }

        [Test]
        public void A_ray_misses_a_CSG_object()
        {
            var csg = new Csg(CsgOperation.Union, Sphere.UnitSphere(), new Cube());
            var ray = new Ray(Tuple.Point(0, 2, -5), Tuple.Vector(0, 0, 1));
            var xs = csg.LocalIntersect(ray);
            xs.Intersections.Should().BeEmpty();
        }

        [Test]
        public void A_ray_hits_a_CSG_object()
        {
            var s1 = Sphere.UnitSphere();
            var s2 = Sphere.UnitSphere();
            s2.Transform = Transformation.Translation(0, 0, 0.5);

            var csg = new Csg(CsgOperation.Union, s1, s2);
            var ray = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            var xs = csg.LocalIntersect(ray);

            xs.Intersections.Should().HaveCount(2);
            xs[0].T.Should().Be(4);
            xs[0].Object.Should().Be(s1);
            xs[1].T.Should().Be(6.5);
            xs[1].Object.Should().Be(s2);
        }
    }
}
