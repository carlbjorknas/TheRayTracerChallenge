using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRayTracerChallenge
{
    class World
    {
        public List<Sphere> Spheres { get; set; } = new List<Sphere>();
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
            world.Spheres.Add(s1);
            world.Spheres.Add(s2);

            return world;
        }

        internal IntersectionCollection Intersect(Ray ray)
        {
            var intersections = Spheres
                .SelectMany(s => s.Intersect(ray).Intersections)
                .OrderBy(i => i.T)
                .ToList();
            return new IntersectionCollection(intersections);
        }

        internal Color ShadeHit(Computations comps)
            => comps.Object.Material.Lightning(LightSource, comps.Point, comps.EyeVector, comps.NormalVector);
    }
}
