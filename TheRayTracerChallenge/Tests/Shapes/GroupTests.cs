﻿using FluentAssertions;
using NUnit.Framework;
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

        [Test]
        public void Adding_a_shape_to_a_group()
        {
            var group = new Group();
            var shape = new TestShape();
            group.AddChild(shape);
            group.Shapes.Count.Should().Be(1);
            group.Shapes.Should().Contain(shape);
            shape.Parent.Should().Be(group);
        }

        [Test]
        public void Intersecting_a_ray_with_an_empty_group()
        {
            var group = new Group();
            var ray = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            var xs = group.LocalIntersect(ray);
            xs.Count.Should().Be(0);
        }

        [Test]
        //This test builds a group of three spheres and casts a ray at it. The
        //spheres are arranged inside the group so that the ray will intersect two of the
        //spheres but miss the third.The resulting collection of intersections should
        //include those of the two spheres.
        public void Intersecting_a_ray_with_a_nonempty_group()
        {
            // Arrange
            var group = new Group();

            var s1 = Sphere.UnitSphere();

            var s2 = Sphere.UnitSphere();
            s2.Transform = Transformation.Translation(0, 0, -3);

            var s3 = Sphere.UnitSphere();
            s3.Transform = Transformation.Translation(5, 0, 0);

            group.AddChild(s1);
            group.AddChild(s2);
            group.AddChild(s3);

            var ray = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));

            // Act
            var xs = group.LocalIntersect(ray);
            
            // Assert
            xs.Count.Should().Be(4);
            xs[0].Object.Should().Be(s2);
            xs[1].Object.Should().Be(s2);
            xs[2].Object.Should().Be(s1);
            xs[3].Object.Should().Be(s1);
        }

        [Test]
        //This test creates a group and adds a single sphere to it.The new group is given
        //one transformation, and the sphere is given a different transformation.A ray is
        //then cast in such a way that it should strike the sphere, as long as the sphere
        //is being transformed by both its own transformation and that of its parent.
        public void Intersecting_a_transformed_group()
        {
            var group = new Group();
            group.Transform = Transformation.Scaling(2, 2, 2);

            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Translation(5, 0, 0);
            group.AddChild(sphere);

            var ray = new Ray(Tuple.Point(10, 0, -10), Tuple.Vector(0, 0, 1));

            var xs = group.Intersect(ray);

            xs.Count.Should().Be(2);
        }
    }
}
