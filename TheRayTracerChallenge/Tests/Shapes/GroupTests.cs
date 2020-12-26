using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.Tests.Shapes
{
    [TestFixture]
    public class GroupTests
    {
        [Test]
        public void Creating_a_new_group()
        {
            var group = new Group();
            group.Transform.Should().Be(Transformation.Identity);
            group.Shapes.Should().BeEmpty();
        }
    }
}
