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
            PrintSphereSilhouette();
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

        private static void PrintSphereSilhouette()
        {                        
            var ray_origin = Tuple.Point(0, 0, -5);
            var wall_z = 10;
            const double wall_size = 7.0;
            const int canvas_pixels = 100;
            var pixel_size = wall_size / canvas_pixels;
            var half = wall_size / 2;

            var canvas = new Canvas(canvas_pixels, canvas_pixels);
            var color = new Color(1, 0, 0);
            var shape = Sphere.UnitSphere();

            for (var y=0; y<canvas_pixels; y++)
            {
                // compute the world y coordinate (top = +half, bottom = -half)
                var world_y = half - pixel_size * y;

                for (var x=0; x<canvas_pixels; x++)
                {
                    // compute the world x coordinate (left = -half, right = half)
                    var world_x = -half + pixel_size * x;

                    // describe the point on the wall that the ray will target
                    var position = Tuple.Point(world_x, world_y, wall_z);

                    var ray = new Ray(ray_origin, (position - ray_origin).Normalize);
                    var intersections = shape.Intersect(ray);

                    if (intersections.Hit() != null)
                    {
                        canvas.WritePixel(x, y, color);
                    }                    
                }
            }


            canvas.WritePixel(0, 0, color);
            File.WriteAllText("sphere_silhoutte.ppm", canvas.ToPpm());
        }
    }
}
