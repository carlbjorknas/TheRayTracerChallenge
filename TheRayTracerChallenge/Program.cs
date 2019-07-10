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
            PrintSphereSilhouttes();
            Print3DSphere();
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

        private static void PrintSphereSilhouttes()
        {
            var sphere = Sphere.UnitSphere();
            PrintSphereSilhouette(sphere, "sphere_silhoutte.ppm");

            sphere.Transform = Transformation.Scaling(1, 0.5, 1);
            PrintSphereSilhouette(sphere, "sphere_silhoutte_scaled.ppm");

            // Shrink and rotate
            var rotation = Transformation.RotationZ(Math.PI / 4);
            var scaling = Transformation.Scaling(0.5, 1, 1);
            sphere.Transform = rotation.Chain(scaling);
            PrintSphereSilhouette(sphere, "sphere_silhoutte_scaled_and_rotated.ppm");

            // shrink and skew
            sphere.Transform = Transformation.Shearing(1, 0, 0, 0, 0, 0)
                .Chain(Transformation.Scaling(0.5, 1, 1));
            PrintSphereSilhouette(sphere, "sphere_silhoutte_scaled_and_skewed.ppm");
        }

        private static void PrintSphereSilhouette(Sphere shape, string filename)
        {                        
            var ray_origin = Tuple.Point(0, 0, -5);
            var wall_z = 10;
            const double wall_size = 7.0;
            const int canvas_pixels = 100;
            var pixel_size = wall_size / canvas_pixels;
            var half = wall_size / 2;

            var canvas = new Canvas(canvas_pixels, canvas_pixels);
            var color = new Color(1, 0, 0);

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
            File.WriteAllText(filename, canvas.ToPpm());
        }

        private static void Print3DSphere()
        {
            var shape = Sphere.UnitSphere();
            shape.Material.Color = new Color(1, 0.2, 1);

            var ray_origin = Tuple.Point(0, 0, -5);
            var wall_z = 10;
            const double wall_size = 7.0;
            const int canvas_pixels = 300;
            var pixel_size = wall_size / canvas_pixels;
            var half = wall_size / 2;

            var light = new PointLight(Tuple.Point(-10, 10, -10), new Color(1, 1, 1));

            var canvas = new Canvas(canvas_pixels, canvas_pixels);

            for (var y = 0; y < canvas_pixels; y++)
            {
                // compute the world y coordinate (top = +half, bottom = -half)
                var world_y = half - pixel_size * y;

                for (var x = 0; x < canvas_pixels; x++)
                {
                    // compute the world x coordinate (left = -half, right = half)
                    var world_x = -half + pixel_size * x;

                    // describe the point on the wall that the ray will target
                    var position = Tuple.Point(world_x, world_y, wall_z);

                    var ray = new Ray(ray_origin, (position - ray_origin).Normalize);
                    var intersections = shape.Intersect(ray);

                    if (intersections.Hit() != null)
                    {
                        var hit = intersections.Hit().Value;
                        var point = ray.Position(hit.T);
                        var normalv = hit.Object.NormalAt(point);
                        var eyev = -ray.Direction;
                        var color = hit.Object.Material.Lightning(light, point, eyev, normalv);
                        canvas.WritePixel(x, y, color);
                    }
                }
            }


            canvas.WritePixel(0, 0, Color.Black);
            File.WriteAllText("sphere_3D.ppm", canvas.ToPpm());
        }
    }
}
