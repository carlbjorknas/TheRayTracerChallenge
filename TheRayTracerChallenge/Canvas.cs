using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRayTracerChallenge
{
    class Canvas
    {
        readonly Color[,] _colors;

        public Canvas(int width, int height)
        {
            _colors = new Color[width, height];
        }

        public int Width => _colors.GetLength(0);

        public int Height => _colors.GetLength(1);

        public Color PixelAt(int x, int y) 
            => _colors[x, y];

        internal void WritePixel(int x, int y, Color c) 
            => _colors[x, y] = c;

        internal void WritePixel(Vector<double> vec, Color c)
        {
            var x = (int)Math.Round(vec[0] + (Width -1 ) / 2);
            var y = (int)Math.Round(Height - 1 - (vec[1] + (Height-1) / 2));
            WritePixel(x, y, c);
        }

        internal string ToPpm()
            => PpmHeader + PpmPixelData + Environment.NewLine;
      
        private string PpmHeader
            => new StringBuilder()
                .AppendLine("P3")
                .AppendLine($"{Width} {Height}")
                .AppendLine("255")
                .ToString();

        private string PpmPixelData
            => string.Join("\r\n", GetAllRows.Select(ConvertToPpmRow));

        private IEnumerable<IEnumerable<Color>> GetAllRows
            => Enumerable.Range(0, Height)
                .Select(rowIndex => GetPixelsInRow(rowIndex));        

        private IEnumerable<Color> GetPixelsInRow(int rowIndex)
            => Enumerable.Range(0, Width)
                .Select(colIndex => PixelAt(colIndex, rowIndex));

        private string ConvertToPpmRow(IEnumerable<Color> colors)
            => string.Join(' ', colors.Select(ConvertToPpmColor))
                .BreakIntoLinesWithMaxLength(70);

        private string ConvertToPpmColor(Color c)
            => $"{PpmValueFor(c.r)} {PpmValueFor(c.g)} {PpmValueFor(c.b)}";

        private int PpmValueFor(double value)
        {
            var ppmValue = (int)Math.Round(value * 255);

            if (ppmValue < 0) return 0;
            if (ppmValue > 255) return 255;
            return ppmValue;
        }
    }
}
