using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class MaterialTests
    {
        Material _m;
        Tuple _position;

        [SetUp]
        public void SetUp()
        {
            _m = new Material();
            _position = Tuple.Point(0, 0, 0);
        }

        [Test]
        public void The_default_material()
        {
            Assert.AreEqual(new Color(1, 1, 1), _m.Color);
            Assert.AreEqual(0.1, _m.Ambient);
            Assert.AreEqual(0.9, _m.Diffuse);
            Assert.AreEqual(0.9, _m.Specular);
            Assert.AreEqual(200.0, _m.Shininess);
        }

        [Test]
        public void Lighting_with_the_eye_between_the_light_and_the_surface()
        {
            var eyev = Tuple.Vector(0, 0, -1);
            var normalv = Tuple.Vector(0, 0, -1);
            var light = new PointLight(Tuple.Point(0, 0, -10), new Color(1, 1, 1));

            var result = _m.Lighting(light, _position, eyev, normalv, false);

            Assert.AreEqual(new Color(1.9, 1.9, 1.9), result);
        }

        [Test]
        public void Lighting_with_the_eye_between_light_and_surface_eye_offset_45_degrees()
        {
            var eyev = Tuple.Vector(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
            var normalv = Tuple.Vector(0, 0, -1);
            var light = new PointLight(Tuple.Point(0, 0, -10), new Color(1, 1, 1));

            var result = _m.Lighting(light, _position, eyev, normalv, false);

            Assert.AreEqual(new Color(1.0, 1.0, 1.0), result);
        }

        [Test]
        public void Lighting_with_eye_opposite_surface_light_offset_45_degrees()
        {
            var eyev = Tuple.Vector(0, 0, -1);
            var normalv = Tuple.Vector(0, 0, -1);
            var light = new PointLight(Tuple.Point(0, 10, -10), new Color(1, 1, 1));

            var result = _m.Lighting(light, _position, eyev, normalv, false);

            Assert.AreEqual(new Color(0.7364, 0.7364, 0.7364), result);
        }

        [Test]
        public void Lighting_with_eye_in_the_path_of_the_reflection_vector()
        {
            var eyev = Tuple.Vector(0, -Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
            var normalv = Tuple.Vector(0, 0, -1);
            var light = new PointLight(Tuple.Point(0, 10, -10), new Color(1, 1, 1));

            var result = _m.Lighting(light, _position, eyev, normalv, false);

            Assert.AreEqual(new Color(1.6364, 1.6364, 1.6364), result);
        }

        [Test]
        public void Lighting_with_the_light_behind_the_surface()
        {
            var eyev = Tuple.Vector(0, 0, -1);
            var normalv = Tuple.Vector(0, 0, -1);
            var light = new PointLight(Tuple.Point(0, 0, 10), new Color(1, 1, 1));

            var result = _m.Lighting(light, _position, eyev, normalv, false);

            Assert.AreEqual(new Color(0.1, 0.1, 0.1), result);
        }

        [Test]
        public void Lighting_with_the_surface_in_shadow()
        {
            var eyeVector = Tuple.Vector(0, 0, -1);
            var normalVector = Tuple.Vector(0, 0, -1);
            var light = new PointLight(Tuple.Point(0, 0, -10), Color.White);
            var inShadow = true;
            var result = _m.Lighting(light, _position, eyeVector, normalVector, inShadow);

            Assert.AreEqual(new Color(0.1, 0.1, 0.1), result);
        }
    }
}
