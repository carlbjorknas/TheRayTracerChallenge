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
            var world = new World();

            world.LightSource = new PointLight(Tuple.Point(-10, 10, -10), Color.White);

            var s1 = Sphere.UnitSphere();
            s1.Material = new Material
            {
                Color = new Color(0.8, 1.0, 0.6),
                Diffuse = 0.7,
                Specular = 0.2
            };
            world.Shapes.Add(s1);

            var s2 = Sphere.UnitSphere();
            s2.Transform = Transformation.Scaling(0.5, 0.5, 0.5);
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
            var surface = comps.Object.Material.Lighting(comps.Object, LightSource, comps.OverPoint, comps.EyeVector, comps.NormalVector, isShadowed);
            var reflected = ReflectedColor(comps, remaining);
            var refracted = RefractedColor(comps, remaining);

            var material = comps.Object.Material;
            if (material.Reflective > 0 && material.Transparency > 0)
            {
                var reflectance = comps.Schlick();
                return surface + 
                    reflected * reflectance + 
                    refracted * (1-reflectance);
            }

            return surface + reflected + refracted;
        }

        internal Color ColorAt(Ray ray, int remaining = 5)
        {
            var intersections = Intersect(ray);
            var hit = intersections.Hit();

            if (hit == null)
            {
                return Color.Black;
            }

            var comps = hit.Value.PrepareComputations(ray, intersections);
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

        internal Color RefractedColor(Computations comps, int remaining)
        {
            if (remaining == 0)
                return Color.Black;

            if (comps.Object.Material.Transparency == 0)
                return Color.Black;

            // Snell's law
            var nRatio = comps.n1 / comps.n2;
            var cosTheta_i = comps.EyeVector.Dot(comps.NormalVector);
            var sinTheta_t_squared = Math.Pow(nRatio, 2) * (1 - Math.Pow(cosTheta_i, 2));

            if (sinTheta_t_squared > 1)
                return Color.Black;

            var cosTheta_t = Math.Sqrt(1 - sinTheta_t_squared);
            var refractedRayDirection =
                comps.NormalVector * (nRatio * cosTheta_i - cosTheta_t) -
                comps.EyeVector * nRatio;
            var refractedRay = new Ray(comps.UnderPoint, refractedRayDirection);
            // Find the color of the refracted ray, making sure to multiply
            // by the transparency value to account for any opacity
            var color = ColorAt(refractedRay, --remaining) * comps.Object.Material.Transparency;

            return color;
        }
    }
}
