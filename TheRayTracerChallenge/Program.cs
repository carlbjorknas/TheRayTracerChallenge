using System;
using System.IO;

namespace TheRayTracerChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //PrintAPixelToACanvas();
            //PrintClockFace();
            //PrintSphereSilhouttes();
            //Print3DSphere();
            //PrintAScene();
            //PrintASceneUsingAPlane();
            PrintASceneWithPattern();
        }

        private static void PrintAPixelToACanvas()
        {
            var canvas = new Canvas(5, 5);
            canvas.WritePixel(1, 1, new Color(1, 1, 1));
            var ppm = canvas.ToPpm();
            SaveImage("single_pixel.ppm", ppm);
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
            SaveImage("clock_face.ppm", ppm);
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
            SaveImage(filename, canvas.ToPpm());
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
                        var color = hit.Object.Material.Lighting(shape, light, point, eyev, normalv, false);
                        canvas.WritePixel(x, y, color);
                    }
                }
            }


            canvas.WritePixel(0, 0, Color.Black);
            SaveImage("sphere_3D.ppm", canvas.ToPpm());
        }

        static void PrintAScene()
        {
            var floor = Sphere.UnitSphere();
            floor.Transform = Transformation.Scaling(10, 0.01, 10);
            floor.Material = new Material
            {
                Color = new Color(1, 0.9, 0.9),
                Specular = 0
            };

            var leftWall = Sphere.UnitSphere();
            leftWall.Transform =
                Transformation.Translation(0, 0, 5)
                    .Chain(Transformation.RotationY(-Math.PI / 4))
                    .Chain(Transformation.RotationX(Math.PI / 2))
                    .Chain(Transformation.Scaling(10, 0.01, 10));                   
            leftWall.Material = floor.Material;

            var rightWall = Sphere.UnitSphere();
            rightWall.Transform =
                Transformation.Translation(0, 0, 5)
                    .Chain(Transformation.RotationY(Math.PI / 4))
                    .Chain(Transformation.RotationX(Math.PI / 2))
                    .Chain(Transformation.Scaling(10, 0.01, 10));
            rightWall.Material = floor.Material;

            var middle = Sphere.UnitSphere();
            middle.Transform = Transformation.Translation(-0.5, 1, 0.5);
            middle.Material = new Material
            {
                Color = new Color(0.1, 1, 0.5),
                Diffuse = 0.7,
                Specular = 0.3
            };

            var right = Sphere.UnitSphere();
            right.Transform = Transformation.Translation(1.5, 0.5, -0.5)
                .Chain(Transformation.Scaling(0.5, 0.5, 0.5));
            right.Material = new Material
            {
                Color = new Color(0.5, 1, 0.1),
                Diffuse = 0.7,
                Specular = 0.3
            };

            var left = Sphere.UnitSphere();
            left.Transform = Transformation.Translation(-1.5, 0.33, -0.75)
                .Chain(Transformation.Scaling(0.33, 0.33, 0.33));
            left.Material = new Material
            {
                Color = new Color(1, 0.8, 0.1),
                Diffuse = 0.7,
                Specular = 0.3
            };

            var world = new World();
            world.LightSource = new PointLight(Tuple.Point(-10, 10, -10), Color.White);
            world.Shapes.Add(floor);
            world.Shapes.Add(leftWall);
            world.Shapes.Add(rightWall);
            world.Shapes.Add(left);
            world.Shapes.Add(middle);
            world.Shapes.Add(right);

            var camera = new Camera(1000, 500, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(0, 1.5, -5),
                Tuple.Point(0, 1, 0),
                Tuple.Point(0, 1, 0));

            var canvas = camera.Render(world);

            SaveImage("scene.ppm", canvas.ToPpm());
        }

        static void PrintASceneUsingAPlane()
        {
            var floor = new Plane();
            floor.Transform = Transformation.Scaling(10, 0.01, 10);
            floor.Material = new Material
            {
                Color = new Color(1, 0.9, 0.9),
                Specular = 0
            };

            var middle = Sphere.UnitSphere();
            middle.Transform = Transformation.Translation(-0.5, 0, 0.5);
            middle.Material = new Material
            {
                Color = new Color(0.1, 1, 0.5),
                Diffuse = 0.7,
                Specular = 0.3
            };

            var right = Sphere.UnitSphere();
            right.Transform = Transformation.Translation(1.5, 0.5, -0.5)
                .Chain(Transformation.Scaling(0.5, 0.5, 0.5));
            right.Material = new Material
            {
                Color = new Color(0.5, 1, 0.1),
                Diffuse = 0.7,
                Specular = 0.3
            };

            var left = Sphere.UnitSphere();
            left.Transform = Transformation.Translation(-1.5, 0.33, -0.75)
                .Chain(Transformation.Scaling(0.33, 0.33, 0.33));
            left.Material = new Material
            {
                Color = new Color(1, 0.8, 0.1),
                Diffuse = 0.7,
                Specular = 0.3
            };

            var world = new World();
            world.LightSource = new PointLight(Tuple.Point(-10, 10, -10), Color.White);
            world.Shapes.Add(floor);
            world.Shapes.Add(left);
            world.Shapes.Add(middle);
            world.Shapes.Add(right);

            var camera = new Camera(1000, 500, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(0, 1.5, -5),
                Tuple.Point(0, 1, 0),
                Tuple.Point(0, 1, 0));

            var canvas = camera.Render(world);

            SaveImage("scene_with_plane.ppm", canvas.ToPpm());
        }

        static void PrintASceneWithPattern()
        {
            var floor = new Plane();
            floor.Material = new Material
            {
                Pattern = new StripePattern(Color.White, new Color(0.2, 0.2, 1)),
                Specular = 0
            };

            var middle = Sphere.UnitSphere();
            middle.Transform = Transformation.Translation(-0.5, 0, 0.5);
            middle.Material = new Material
            {
                Color = new Color(0.1, 1, 0.5),
                Diffuse = 0.7,
                Specular = 0.3
            };

            var right = Sphere.UnitSphere();
            right.Transform = Transformation.Translation(1.5, 0.5, -0.5)
                .Chain(Transformation.Scaling(0.5, 0.5, 0.5))
                .Chain(Transformation.RotationZ(Math.PI / 4))
                .Chain(Transformation.RotationY(Math.PI / 4));                
            right.Material = new Material
            {
                Pattern = new StripePattern(Color.Black, new Color(0.5, 1, 0.1))
                {
                    Transform = Transformation.Scaling(0.1, 0.1, 0.1)
                },                
                Diffuse = 0.7,
                Specular = 0.3
            };

            var left = Sphere.UnitSphere();
            left.Transform = Transformation.Translation(-1.5, 0.33, -0.75)
                .Chain(Transformation.Scaling(0.33, 0.33, 0.33));
            left.Material = new Material
            {
                Color = new Color(1, 0.8, 0.1),
                Diffuse = 0.7,
                Specular = 0.3
            };

            var world = new World();
            world.LightSource = new PointLight(Tuple.Point(-10, 10, -10), Color.White);
            world.Shapes.Add(floor);
            world.Shapes.Add(left);
            world.Shapes.Add(middle);
            world.Shapes.Add(right);

            var camera = new Camera(1000, 500, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(0, 1.5, -5),
                Tuple.Point(0, 1, 0),
                Tuple.Point(0, 1, 0));

            var canvas = camera.Render(world);

            SaveImage("scene_with_patterned_plane.ppm", canvas.ToPpm());
        }

        private static void SaveImage(string name, string ppm)
        {
            File.WriteAllText("..\\..\\..\\Images\\" + name, ppm);
        }
    }
}
