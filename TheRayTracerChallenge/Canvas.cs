using System;
using System.Collections.Generic;
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
    }
}
