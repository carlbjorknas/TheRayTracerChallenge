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
    }
}
