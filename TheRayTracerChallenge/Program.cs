using System;
using System.IO;
using System.Linq;
using TheRayTracerChallenge.Patterns;
using TheRayTracerChallenge.Scenes;
using TheRayTracerChallenge.Shapes;
using TheRayTracerChallenge.Utils;

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
            //PrintASceneWithPattern();
            //TestingPatterns();
            //RadialGradientFloor();
            //NestedPatternFloor();
            //BlendedPatternFloor();
            //PrintReflectionScene();
            //PrintReflectionAndRefractionScene();
            //PrintCubeTable();
            //new SnowflakeScene("scene_with_snowflakes").Render();
            //new CowFromObjFileScene("scene_with_cow_from_obj_file").Render();
            new TreeFromObjFileScene("scene_with_tree_from_obj_file").Render();
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

        static void TestingPatterns()
        {
            var floor = new Plane()
            {                
                Material = new Material
                {
                    Pattern = new CheckerPattern(Color.White, new Color(0.2, 0.2, 1))
                    {
                        Transform = Transformation.Translation(0, 0.1, 0)
                    },
                    Specular = 0
                }
            };

            var middle = Sphere.UnitSphere();
            middle.Transform = Transformation.Translation(-0.5, 0, 0.5);
            middle.Material = new Material
            {
                Pattern = new StripePattern(new Color(0.1, 1, 0.5), new Color(1, 0.1, 0.5))
                {
                    Transform = Transformation.Scaling(0.5, 0.5, 0.5)
                },
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
                Pattern = new RingPattern(Color.Black, new Color(0.5, 1, 0.1))
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
                Pattern = new GradientPattern(new Color(1, 0.8, 0.1), new Color(0.1, 1, 0.8))
                {
                    Transform = Transformation.Translation(1, 0, 0)
                        .Chain(Transformation.Scaling(2, 1, 1))
                },
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

            SaveImage("testing_patterns.ppm", canvas.ToPpm());
        }

        static void RadialGradientFloor()
        {
            var floor = new Plane()
            {
                Material = new Material
                {
                    Pattern = new RadialGradientPattern(Color.White, new Color(0.2, 0.2, 1)),
                    Specular = 0
                }
            };

            var world = new World();
            world.LightSource = new PointLight(Tuple.Point(-10, 10, -10), Color.White);
            world.Shapes.Add(floor);

            var camera = new Camera(1000, 500, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(0, 5, -5),
                Tuple.Point(0, 1, 0),
                Tuple.Point(0, 1, 0));

            var canvas = camera.Render(world);

            SaveImage("radial_gradient.ppm", canvas.ToPpm());
        }

        static void NestedPatternFloor()
        {
            var floor = new Plane()
            {
                Material = new Material
                {
                    Pattern = new CheckerPattern(
                        new RadialGradientPattern(Color.White, new Color(0.2, 0.2, 1)),
                        new StripePattern(Color.Black, new Color(1, 0, 0))
                        {
                            Transform = Transformation.RotationY(Math.PI / 4)
                                .Chain(Transformation.Scaling(0.1, 1, 1))
                        })
                    { Transform = Transformation.Translation(0, 0.1, 0) },
                    Specular = 0
                }
            };

            var wall = new Plane()
            {
                Material = new Material
                {
                    Pattern = 
                        new RingPattern(
                            new RadialGradientPattern(Color.White, new Color(0.2, 0.2, 1)),
                            new StripePattern(Color.Black, new Color(0, 1, 0))
                            {
                                Transform = Transformation.RotationY(Math.PI / 4)
                                    .Chain(Transformation.Scaling(0.1, 1, 1))
                            })
                        {
                            Transform = Transformation.Translation(0, 0.1, 0)
                        },
                    Specular = 0
                },
                Transform = Transformation.Translation(0, 0, 10)
                    .Chain(Transformation.RotationX(Math.PI / 2))
            };

            var world = new World();
            world.LightSource = new PointLight(Tuple.Point(-10, 10, -10), Color.White);
            world.Shapes.Add(floor);
            world.Shapes.Add(wall);

            var camera = new Camera(1000, 500, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(0, 2, -5),
                Tuple.Point(0, 1, 0),
                Tuple.Point(0, 1, 0));

            var canvas = camera.Render(world);

            SaveImage("nested_patterns.ppm", canvas.ToPpm());
        }

        static void BlendedPatternFloor()
        {
            var floor = new Plane()
            {
                Material = new Material
                {
                    Pattern = new BlendedPattern(
                        new RadialGradientPattern(Color.White, new Color(0, 0, 0)),
                        new StripePattern(Color.Black, new Color(1, 0, 0))
                        {
                            Transform = Transformation.RotationY(Math.PI / 4)
                                .Chain(Transformation.Scaling(0.5, 1, 1))
                        })
                    { Transform = Transformation.Translation(0, 0.1, 0) },
                    Specular = 0
                }
            };

            var world = new World();
            world.LightSource = new PointLight(Tuple.Point(-10, 10, -10), Color.White);
            world.Shapes.Add(floor);

            var camera = new Camera(1000, 500, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(0, 2, -5),
                Tuple.Point(0, 1, 0),
                Tuple.Point(0, 1, 0));

            var canvas = camera.Render(world);

            SaveImage("blended_patterns.ppm", canvas.ToPpm());
        }

        public static void PrintReflectionScene()
        {
            var floor = new Plane();
            floor.Transform = Transformation.Scaling(10, 0.01, 10);
            floor.Material = new Material
            {
                Color = new Color(1, 0.9, 0.9),
                Specular = 0,
                Reflective = 0.4
            };

            var middle = Sphere.UnitSphere();
            middle.Transform = Transformation.Translation(-0.5, 1, 0.5);
            middle.Material = new Material
            {
                Color = new Color(0.1, 1, 0.5),
                Diffuse = 0.7,
                Specular = 0.3,
                Reflective = 0.5
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
                Specular = 0.3,
                Reflective = 0.1
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

            SaveImage("scene_with_reflections.ppm", canvas.ToPpm());
        }

        public static void PrintReflectionAndRefractionScene()
        {
            var backWall = new Plane()
            {
                Transform = Transformation.RotationX(Math.PI / 2)
                    .Chain(Transformation.Translation(0, 10, 0)),
                Material = new Material
                {
                    Pattern = new CheckerPattern(Color.White, Color.Black)
                }
            };

            var floor = new Plane()
            {
                Transform = Transformation.Translation(0, -1, 0)
                    .Chain(Transformation.RotationY(Math.PI / 4))
                    .Chain(Transformation.Scaling(0.8, 1, 1)),
                Material = new Material
                {
                    Pattern = new StripePattern(Color.White, Color.Black)
                }
            };

            var purpleGlassSphere = Sphere.Glass();
            purpleGlassSphere.Material.Color = new Color(0.8, 0, 0.8);
            purpleGlassSphere.Transform = Transformation.Translation(-1.2, 0, 0);

            var blueGlassSphere = Sphere.Glass();
            blueGlassSphere.Material.Color = new Color(0, 0, 0.8);
            blueGlassSphere.Material.Ambient = 0;
            blueGlassSphere.Material.Diffuse = 0;
            blueGlassSphere.Material.Specular = 0.9;
            blueGlassSphere.Material.Shininess = 300;
            blueGlassSphere.Material.Reflective = 0.9;
            blueGlassSphere.Transform = Transformation.Translation(1.2, 0, 0);

            var airBubbleInsideBlueGlassSphere = Sphere.UnitSphere();
            airBubbleInsideBlueGlassSphere.Material.Color = Color.White;
            airBubbleInsideBlueGlassSphere.Material.Specular = 0.9;
            airBubbleInsideBlueGlassSphere.Material.Shininess = 300;
            airBubbleInsideBlueGlassSphere.Material.Reflective = 0.9;
            airBubbleInsideBlueGlassSphere.Material.Transparency = 0.9;
            airBubbleInsideBlueGlassSphere.Material.RefractiveIndex = RefractiveIndex.Air;
            airBubbleInsideBlueGlassSphere.Transform = Transformation.Translation(1.2, 0, 0)
                .Chain(Transformation.Scaling(0.3, 0.3, 0.3));

            var totallyReflectiveSphere = Sphere.UnitSphere();
            totallyReflectiveSphere.Transform = Transformation.Translation(0, 0, 5);
            totallyReflectiveSphere.Material.Color = Color.Red;
            totallyReflectiveSphere.Material.Reflective = 1;
            totallyReflectiveSphere.Material.Transparency = 0;
            totallyReflectiveSphere.Material.Shininess = 1000;

            var yellowTransparent = Sphere.UnitSphere();
            yellowTransparent.Transform = Transformation.Translation(5, 2, 3)
                .Chain(Transformation.Scaling(0.6, 1, 0.6));
            yellowTransparent.Material = new Material
            {
                Color = new Color(0.8, 0.8, 0),
                Reflective = 0.05,
                Transparency = 0.95
            };

            var world = new World();
            world.LightSource = new PointLight(Tuple.Point(-10, 10, -10), Color.White);
            world.Shapes.Add(backWall);
            world.Shapes.Add(floor);
            world.Shapes.Add(purpleGlassSphere);
            world.Shapes.Add(blueGlassSphere);
            world.Shapes.Add(airBubbleInsideBlueGlassSphere);
            world.Shapes.Add(totallyReflectiveSphere);
            world.Shapes.Add(yellowTransparent);

            var camera = new Camera(1000, 500, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(0, 0, -5),
                Tuple.Point(0, 0, 0),
                Tuple.Point(0, 1, 0));

            var canvas = camera.Render(world);

            SaveImage("scene_with_reflection_and_refraction.ppm", canvas.ToPpm());
        }

        private static void PrintCubeTable()
        {
            var tableMaterial = new Material { Color = new Color(1, 0.8, 0.25) };

            var tableSurface = new Cube();
            tableSurface.Material = tableMaterial;
            tableSurface.Transform = Transformation.Scaling(5, 0.1, 3);

            var leftFrontLeg = CreateLeg();
            leftFrontLeg.Transform = Transformation.Translation(-4.6, -3, -2.6).Chain(leftFrontLeg.Transform);

            var leftBackLeg = CreateLeg();
            leftBackLeg.Transform = Transformation.Translation(-4.6, -3, 2.6).Chain(leftBackLeg.Transform);

            var rightFrontLeg = CreateLeg();
            rightFrontLeg.Transform = Transformation.Translation(4.6, -3, -2.6).Chain(rightFrontLeg.Transform);

            var rightBackLeg = CreateLeg();
            rightBackLeg.Transform = Transformation.Translation(4.6, -3, 2.6).Chain(rightBackLeg.Transform);

            // The light bulb idea doesn't seem to work. I guess the handling of shadows is too basic.
            // It doesn't seem that shadowing care about transparency, which means a glass bulb doesn't let any light through
            // to the objects it shadows.            
            //var lamp = new Sphere(Tuple.Point(3, 5, 0));
            //lamp.Material = new Material
            //{
            //    Transparency = 1,
            //    Diffuse = 0.1,
            //    Reflective = 0.5,
            //    Ambient = 0.5,
            //    Specular = 0.5,
            //    Shininess = 0.5,
            //    Color = new Color(1, 0, 0),
            //    RefractiveIndex = RefractiveIndex.Air
            //};

            var floor = new Plane();
            floor.Transform = Transformation.Translation(0, -6, 0);

            var backWall = new Plane();
            backWall.Transform = Transformation.Translation(0, 0, 20)
                .Chain(Transformation.RotationX(Math.PI / 2));
            backWall.Material.Pattern = new StripePattern(new Color(0.6, 0.6, 1), new Color(0.9, 0.9, 1));

            var rightWall = new Plane();
            rightWall.Transform = Transformation.Translation(5, 0, 0)
                .Chain(Transformation.RotationY(Math.PI / 2))
                .Chain(Transformation.RotationX(Math.PI / 2));
                
            rightWall.Material.Pattern = new StripePattern(new Color(0.6, 0.6, 1), new Color(0.9, 0.9, 1));

            var world = new World();
            world.LightSource = new PointLight(Tuple.Point(-10, 20, 0), Color.White);
            world.Shapes.AddRange(new Shape[] { 
                tableSurface, 
                leftFrontLeg, leftBackLeg,
                rightFrontLeg, rightBackLeg,
                //lamp,
                floor, backWall, rightWall
            });

            var camera = new Camera(1000, 500, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(
                Tuple.Point(-4, 2, -15),
                Tuple.Point(0, 0, 0),
                Tuple.Point(0, 1, 0));

            var canvas = camera.Render(world);

            SaveImage("scene_with_table_made_of_cubes.ppm", canvas.ToPpm());

            Cube CreateLeg()
            {
                var leg = new Cube();
                leg.Transform = Transformation.Scaling(0.2, 3, 0.2);
                leg.Material = tableMaterial;
                return leg;
            }
        }

        private static void SaveImage(string name, string ppm)
        {
            File.WriteAllText("..\\..\\..\\Images\\" + name, ppm);
        }
    }
}
