using System;
using System.IO;

namespace TheRayTracerChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var canvas = new Canvas(5, 5);
            canvas.WritePixel(1, 1, new Color(1, 1, 1));
            var ppm = canvas.ToPpm();
            File.WriteAllText("test.ppm", ppm);
        }
    }
}
