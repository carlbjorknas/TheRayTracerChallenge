using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class CameraTests
    {
        [Test]
        public void Contructing_a_camera()
        {
            var hSize = 160;
            var vSize = 120;
            var fieldOfView = Math.PI / 2;

            var camera = new Camera(hSize, vSize, fieldOfView);

            Assert.AreEqual(hSize, camera.HSize);
            Assert.AreEqual(vSize, camera.VSize);
            Assert.AreEqual(fieldOfView, camera.FieldOfView);
            Assert.AreEqual(Transformation.Identity, camera.Transform);
        }

        [Test]
        public void The_pixel_size_for_a_horizontal_canvas()
        {
            var camera = new Camera(200, 125, Math.PI / 2);
            Assert.AreEqual(0.01, camera.PixelSize, C.Epsilon);
        }

        [Test]
        public void The_pixel_size_for_a_vertical_canvas()
        {
            var camera = new Camera(125, 200, Math.PI / 2);
            Assert.AreEqual(0.01, camera.PixelSize, C.Epsilon);
        }

        [Test]
        public void Constructing_a_ray_through_the_center_of_the_canvas()
        {
            var camera = new Camera(201, 101, Math.PI / 2);
            var ray = camera.RayForPixel(100, 50);

            Assert.AreEqual(Tuple.Point(0, 0, 0), ray.Point);
            Assert.AreEqual(Tuple.Vector(0, 0, -1), ray.Direction);
        }

        [Test]
        public void Constructing_a_ray_through_a_corner_of_the_canvas()
        {
            var camera = new Camera(201, 101, Math.PI / 2);
            var ray = camera.RayForPixel(0, 0);

            Assert.AreEqual(Tuple.Point(0, 0, 0), ray.Point);
            Assert.AreEqual(Tuple.Vector(0.66519, 0.33259, -0.66851), ray.Direction);
        }

        [Test]
        public void Constructing_a_ray_when_the_camera_is_transformed()
        {
            var camera = new Camera(201, 101, Math.PI / 2);
            camera.Transform = Transformation.RotationY(Math.PI / 4).Chain(Transformation.Translation(0, -2, 5));
            var ray = camera.RayForPixel(100, 50);

            Assert.AreEqual(Tuple.Point(0, 2, -5), ray.Point);
            Assert.AreEqual(Tuple.Vector(Math.Sqrt(2) / 2, 0, -Math.Sqrt(2) / 2), ray.Direction);
        }

        [Test]
        public void Rendering_a_world_with_the_camera()
        {
            var world = World.Default();
            var camera = new Camera(11, 11, Math.PI / 2);
            var from = Tuple.Point(0, 0, -5);
            var to = Tuple.Point(0, 0, 0);
            var up = Tuple.Vector(0, 1, 0);
            camera.Transform = Transformation.ViewTransform(from, to, up);

            var image = camera.Render(world);

            Assert.AreEqual(new Color(0.38066, 0.47583, 0.2855), image.PixelAt(5, 5));
        }
    }
}
