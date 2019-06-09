using System;
using System.IO;
using System.Threading;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;

namespace TheRayTracerChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            PrintClockFace();
        }

        private static void PrintAPixelToACanvas()
        {
            var canvas = new Canvas(5, 5);
            canvas.WritePixel(1, 1, new Color(1, 1, 1));
            var ppm = canvas.ToPpm();
            File.WriteAllText("test.ppm", ppm);
        }

        private static void PrintClockFace()
        {
            var canvas = new Canvas(201, 201);
            var white = new Color(1, 1, 1);

            var V = Vector<double>.Build.Dense(new double[] { 0, 80, 0 });
            canvas.WritePixel(V, white);

            for (int i = 0; i < 11; i++) {
                var rotMatrix = Matrix3D.RotationAroundZAxis(-Angle.FromRadians(Math.PI / 6));
                V = rotMatrix * V;
                canvas.WritePixel(V, white);
            }

            var ppm = canvas.ToPpm();
            File.WriteAllText("clock.ppm", ppm);
        }
    }
}
