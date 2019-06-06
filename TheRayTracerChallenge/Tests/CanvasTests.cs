using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class CanvasTests
    {
        [Test]
        public void Creating_a_canvas()
        {
            var canvas = new Canvas(10, 20);
            Assert.AreEqual(10, canvas.Width);
            Assert.AreEqual(20, canvas.Height);

            var black = new Color(0, 0, 0);
            var cols = Enumerable.Range(0, 10);
            var rows = Enumerable.Range(0, 20);
            cols
                .SelectMany(x => rows, (x, y) => new { x, y })
                .Select(pair => canvas.PixelAt(pair.x, pair.y))
                .ToList()
                .ForEach(pixelColor => Assert.AreEqual(black, pixelColor));
        }

        [Test]
        public void Writing_pixels_to_a_canvas()
        {
            var canvas = new Canvas(10, 20);
            var red = new Color(1, 0, 0);
            canvas.WritePixel(2, 3, red);
            Assert.AreEqual(red, canvas.PixelAt(2, 3));
        }

        [Test]
        public void Constructing_the_PPM_header()
        {
            var canvas = new Canvas(5, 3);
            var ppm = canvas.ToPpm();
            var header = new StringBuilder()
                .AppendLine("P3")
                .AppendLine("5 3")
                .AppendLine("255")
                .ToString();
            Assert.IsTrue(ppm.StartsWith(header));
        }

        [Test]
        public void Constructing_the_PPM_pixel_data()
        {
            var canvas = new Canvas(5, 3);
            var c1 = new Color(1.5, 0, 0);
            var c2 = new Color(0, 0.5, 0);
            var c3 = new Color(-0.5, 0, 1);

            canvas.WritePixel(0, 0, c1);
            canvas.WritePixel(2, 1, c2);
            canvas.WritePixel(4, 2, c3);

            var ppm = canvas.ToPpm();
            var ppmLines = ppm.Split("\r\n");
            Assert.AreEqual("255 0 0 0 0 0 0 0 0 0 0 0 0 0 0", ppmLines[3]);
            Assert.AreEqual("0 0 0 0 0 0 0 128 0 0 0 0 0 0 0", ppmLines[4]);
            Assert.AreEqual("0 0 0 0 0 0 0 0 0 0 0 0 0 0 255", ppmLines[5]);
        }
    }
}
