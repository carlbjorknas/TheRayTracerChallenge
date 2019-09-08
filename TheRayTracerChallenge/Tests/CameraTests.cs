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
        public void Contruction_a_camera()
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
    }
}
