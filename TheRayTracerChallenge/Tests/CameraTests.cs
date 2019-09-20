using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
            Assert.AreEqual(0.01, camera.PixelSize, 0.00001);
        }

        [Test]
        public void The_pixel_size_for_a_vertical_canvas()
        {
            var camera = new Camera(125, 200, Math.PI / 2);
            Assert.AreEqual(0.01, camera.PixelSize, 0.00001);
        }
    }
}
