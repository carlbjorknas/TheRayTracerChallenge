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
    }
}
