using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.Tests.Shapes
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

            Assert.AreEqual(Tuple.Point(0,0,-2.5), shape.LocalRay.Origin);
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

            Assert.AreEqual(Tuple.Point(-5, 0, -5), shape.LocalRay.Origin);
            Assert.AreEqual(Tuple.Vector(0, 0, 1), shape.LocalRay.Direction);
        }

        [Test]
        public void Computing_the_normal_on_a_translated_shape()
        {
            var shape = new TestShape();
            shape.Transform = Transformation.Translation(0, 1, 0);
            var normal = shape.NormalAt(Tuple.Point(0, 1.70711, -0.70711));

            Assert.AreEqual(Tuple.Vector(0, 0.70711, -0.70711), normal);
        }

        [Test]
        public void Computing_the_normal_on_a_transformed_shape()
        {
            var shape = new TestShape();
            shape.Transform = Transformation.Scaling(1, 0.5, 1)
                .Chain(Transformation.RotationZ(Math.PI/5));
            var normal = shape.NormalAt(Tuple.Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2));

            Assert.AreEqual(Tuple.Vector(0, 0.97014, -0.24254), normal);
        }

        [Test]
        public void A_shape_has_a_parent_attribute()
        {
            var shape = new TestShape();
            Assert.IsNull(shape.Parent);
        }

        [Test]
        //This test constructs an outer group, which contains an inner group, which in
        //turn contains a sphere.Each is given its own transformation before calling a new
        //function, world_to_object(shape, point), to convert a world-space point to object space.
        public void Converting_a_point_from_world_to_object_space()
        {
            var outerGroup = new Group();
            outerGroup.Transform = Transformation.RotationY(Math.PI / 2);

            var innerGroup = new Group();
            innerGroup.Transform = Transformation.Scaling(2, 2, 2);
            outerGroup.AddChild(innerGroup);

            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Translation(5, 0, 0);
            innerGroup.AddChild(sphere);

            var objectSpacePoint = sphere.WorldToObject(Tuple.Point(-2, 0, -10));

            Assert.AreEqual(Tuple.Point(0, 0, -1), objectSpacePoint);
        }

        [Test]
        // This sets up two nested groups like in the previous test. Again, each is given
        // its own transformation, and then another new function, normal_to_world(shape, normal), 
        // is used to transform a vector to world space.
        public void Converting_a_normal_from_object_to_world_space()
        {
            var outerGroup = new Group();
            outerGroup.Transform = Transformation.RotationY(Math.PI / 2);

            var innerGroup = new Group();
            innerGroup.Transform = Transformation.Scaling(1, 2, 3);
            outerGroup.AddChild(innerGroup);

            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Translation(5, 0, 0);
            innerGroup.AddChild(sphere);

            var vectorValue = Math.Sqrt(3) / 3;
            var objectSpacePoint = sphere.NormalToWorld(Tuple.Vector(vectorValue, vectorValue, vectorValue));

            Assert.AreEqual(Tuple.Vector(0.28571, 0.42857, -0.85714), objectSpacePoint);
        }

        [Test]
        public void Finding_the_normal_on_a_child_object()
        {
            var outerGroup = new Group();
            outerGroup.Transform = Transformation.RotationY(Math.PI / 2);

            var innerGroup = new Group();
            innerGroup.Transform = Transformation.Scaling(1, 2, 3);
            outerGroup.AddChild(innerGroup);

            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Translation(5, 0, 0);
            innerGroup.AddChild(sphere);

            var normal = sphere.NormalAt(Tuple.Point(1.7321, 1.1547, -5.5774));

            Assert.AreEqual(Tuple.Vector(0.28570, 0.42854, -0.85716), normal);
        }
    }
}
