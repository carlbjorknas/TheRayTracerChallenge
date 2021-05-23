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
            
            var csg = new Csg(Operation.Union, s1, s2);

            csg.Left.Should().Be(s1);
            csg.Right.Should().Be(s2);
            s1.Parent.Should().Be(csg);
            s2.Parent.Should().Be(csg);
        }
    }
}
