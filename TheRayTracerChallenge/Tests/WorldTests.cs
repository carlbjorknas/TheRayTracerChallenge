using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class WorldTests
    {
        [Test]
        public void Creating_a_world()
        {
            var w = new World();
            Assert.IsEmpty(w.Spheres);
            Assert.IsNull(w.LightSource);
        }

        [Test]
        public void The_default_world()
        {
            var light = new PointLight(Tuple.Point(-10, 10, -10), Color.White);

            var s1 = Sphere.UnitSphere();
            s1.Material = new Material
            {
                Color = new Color(0.8, 1.0, 0.6),
                Diffuse = 0.7,
                Specular = 0.2
            };

            var s2 = Sphere.UnitSphere();
            s2.Transform = Transformation.Scaling(0.5, 0.5, 0.5);

            var world = World.Default();
            Assert.AreEqual(light, world.LightSource);
            Assert.Contains(s1, world.Spheres);
        }

        [Test]
        public void Intersect_the_default_world_with_a_ray()
        {
            var world = World.Default();
            var ray = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));

            var intersections = world.Intersect(ray);

            Assert.AreEqual(4, intersections.Count);
            Assert.AreEqual(4, intersections[0].T);
            Assert.AreEqual(4.5, intersections[1].T);
            Assert.AreEqual(5.5, intersections[2].T);
            Assert.AreEqual(6, intersections[3].T);
        }

        [Test]
        public void Shading_an_intersection()
        {
            var world = World.Default();
            var ray = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));
            var shape = world.Spheres.First();
            var i = new Intersection(4, shape);
            var comps = i.PrepareComputations(ray);

            var color = world.ShadeHit(comps);

            Assert.AreEqual(new Color(0.38066, 0.47583, 0.2855), color);
        }

        [Test]
        public void Shading_an_intersection_from_the_inside()
        {
            var world = World.Default();
            world.LightSource = new PointLight(Tuple.Point(0, 0.25, 0), Color.White);
            var ray = new Ray(Tuple.Point(0, 0, 0), Tuple.Vector(0, 0, 1));
            var shape = world.Spheres.Skip(1).First();
            var i = new Intersection(0.5, shape);
            var comps = i.PrepareComputations(ray);

            var color = world.ShadeHit(comps);

            Assert.AreEqual(new Color(0.90498, 0.90498, 0.90498), color);
        }

        [Test]
        public void The_color_when_a_ray_misses()
        {
            var world = World.Default();
            var ray = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 1, 0));

            var color = world.ColorAt(ray);

            Assert.AreEqual(Color.Black, color);
        }

        [Test]
        public void The_color_when_a_ray_hits()
        {
            var world = World.Default();
            var ray = new Ray(Tuple.Point(0, 0, -5), Tuple.Vector(0, 0, 1));

            var color = world.ColorAt(ray);

            Assert.AreEqual(new Color(0.38066, 0.47583, 0.2855), color);
        }

        [Test]
        public void The_color_with_an_intersection_behind_the_ray()
        {
            var world = World.Default();
            var outer = world.Spheres.First();
            outer.Material.Ambient = 1;
            var inner = world.Spheres.Skip(1).First();
            inner.Material.Ambient = 1;
            var ray = new Ray(Tuple.Point(0, 0, 0.75), Tuple.Vector(0, 0, -1));

            var color = world.ColorAt(ray);

            Assert.AreEqual(inner.Material.Color, color);
        }

        [Test]
        public void There_is_no_shadow_when_nothing_is_collinear_with_the_point_and_light()
        {
            var world = World.Default();
            var point = Tuple.Point(0, 10, 0);
            Assert.IsFalse(world.IsShadowed(point));
        }

        [Test]
        public void In_shadow_when_an_object_is_between_the_point_and_the_light()
        {
            var world = World.Default();
            var point = Tuple.Point(10, -10, 10);
            Assert.IsTrue(world.IsShadowed(point));
        }

        [Test]
        public void There_is_no_shadow_when_an_object_is_behind_the_light()
        {
            var world = World.Default();
            var point = Tuple.Point(-20, 20, -20);
            Assert.IsFalse(world.IsShadowed(point));
        }

        [Test]
        public void There_is_no_shadow_when_an_object_is_behind_the_point()
        {
            var world = World.Default();
            var point = Tuple.Point(-2, 2, -2);
            Assert.IsFalse(world.IsShadowed(point));
        }
    }
}
