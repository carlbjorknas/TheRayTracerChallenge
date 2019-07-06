using MathNet.Spatial.Euclidean;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class RayTests
    {
        [Test]
        public void Computing_a_point_from_a_distance()
        {
            var ray = new Ray3D(
                new Point3D(2, 3, 4), 
                new Vector3D(1, 0, 0));

            Assert.AreEqual(new Point3D(2, 3, 4), ray.Position(0));
            Assert.AreEqual(new Point3D(3, 3, 4), ray.Position(1));
            Assert.AreEqual(new Point3D(1, 3, 4), ray.Position(-1));
            Assert.AreEqual(new Point3D(4.5, 3, 4), ray.Position(2.5));
        }
    }
}
