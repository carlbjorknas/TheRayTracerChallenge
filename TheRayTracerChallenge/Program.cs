using System;
using System.IO;

namespace TheRayTracerChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            PrintAPixelToACanvas();
            PrintClockFace();
        }

        private static void PrintAPixelToACanvas()
        {
            var canvas = new Canvas(5, 5);
            canvas.WritePixel(1, 1, new Color(1, 1, 1));
            var ppm = canvas.ToPpm();
            File.WriteAllText("single_pixel.ppm", ppm);
        }

        private static void PrintClockFace()
        {
            var canvas = new Canvas(201, 201);
            var white = new Color(1, 1, 1);

            var V = Tuple.Vector(0, 80, 0);
            canvas.WritePixel(V, white);

            var rotationTransform = Transformation.RotationZ(Math.PI / 6);

            for (int i = 0; i < 11; i++) {
                V = rotationTransform.Transform(V);
                canvas.WritePixel(V, white);
            }

            var ppm = canvas.ToPpm();
            File.WriteAllText("clock_face.ppm", ppm);
        }
    }
}
