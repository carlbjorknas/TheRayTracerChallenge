﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
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

        //[Test]
        //public void 
    }
}