using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRayTracerChallenge
{
    class World
    {
        public List<Shape> Shapes { get; set; } = new List<Shape>();
        public PointLight LightSource { get; set; }

        public static World Default()
        {
            var light = new PointLight(Tuple.Point(-10, 10, -10), Color.White);

            var s1 = Sphere.UnitSphere();
            s1.Material = new Material
            {
                Color = new Color(0.8, 1.0, 0.6),
                Diffuse = 0.7,
                Specular = 0.2
            };

            var s2 = Sphere.UnitSphere();
            s2.Transform = Transformation.Scaling(0.5, 0.5, 0.5);

            var world = new World();
            world.LightSource = light;
            world.Shapes.Add(s1);
            world.Shapes.Add(s2);

            return world;
        }

        internal IntersectionCollection Intersect(Ray ray)
        {
            var intersections = Shapes
                .SelectMany(s => s.Intersect(ray).Intersections)
                .OrderBy(i => i.T)
                .ToList();
            return new IntersectionCollection(intersections);
        }

        internal Color ShadeHit(Computations comps, int remaining = 1)
        {
            var isShadowed = IsShadowed(comps.OverPoint);
            var surface = comps.Object.Material.Lighting(comps.Object, LightSource, comps.Point, comps.EyeVector, comps.NormalVector, isShadowed);
            var reflected = ReflectedColor(comps, remaining);
            return surface + reflected;
        }

        internal Color ColorAt(Ray ray, int remaining = 5)
        {
            var intersections = Intersect(ray);
            var hit = intersections.Hit();

            if (hit == null)
            {
                return Color.Black;
            }

            var comps = hit.Value.PrepareComputations(ray);
            return ShadeHit(comps, remaining);
        }

        internal bool IsShadowed(Tuple point)
        {
            var toLightVector = LightSource.Position - point;            
            var rayToLight = new Ray(point, toLightVector.Normalize);
            var intersections = Intersect(rayToLight);

            var hit = intersections.Hit();
            var distance = toLightVector.Magnitude;
            return hit.HasValue && hit.Value.T < distance;            
        }

        internal Color ReflectedColor(Computations comps, int remaining = 1)
        {
            if (remaining < 1)
                return Color.Black;

            if (comps.Object.Material.Reflective == 0.0)
                return Color.Black;

            var reflectRay = new Ray(comps.OverPoint, comps.ReflectV);
            var color = ColorAt(reflectRay, --remaining);
            return color * comps.Object.Material.Reflective;
        }
    }
}
