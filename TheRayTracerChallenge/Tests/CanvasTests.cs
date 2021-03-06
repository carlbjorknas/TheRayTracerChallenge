﻿using NUnit.Framework;
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
            GetAllCoordsIn(canvas)
                .Select(coords => canvas.PixelAt(coords.x, coords.y))
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

        [Test]
        public void Splitting_long_lines_in_PPM_files()
        {
            var canvas = new Canvas(10, 2);
            var color = new Color(1, 0.8, 0.6);
            GetAllCoordsIn(canvas)
                .ToList()
                .ForEach(coord => canvas.WritePixel(coord.x, coord.y, color));

            var ppm = canvas.ToPpm();
            var ppmLines = ppm.Split("\r\n");
            Assert.AreEqual("255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204", ppmLines[3]);
            Assert.AreEqual("153 255 204 153 255 204 153 255 204 153 255 204 153", ppmLines[4]);
            Assert.AreEqual("255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204", ppmLines[5]);
            Assert.AreEqual("153 255 204 153 255 204 153 255 204 153 255 204 153", ppmLines[6]);
        }

        [Test]
        public void PPM_files_are_terminated_by_a_newline_character()
        {
            var canvas = new Canvas(5, 3);
            var ppm = canvas.ToPpm();
            Assert.AreEqual('\n', ppm.Last());
        }

        private IEnumerable<(int x, int y)> GetAllCoordsIn(Canvas c)
        {
            var cols = Enumerable.Range(0, c.Width);
            var rows = Enumerable.Range(0, c.Height);
            return cols.SelectMany(x => rows, (x, y) => (x, y));
        }
    }
}
