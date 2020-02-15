using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRayTracerChallenge
{
    internal struct Intersection
    {
        public Intersection(double t, Shape o)
        {
            T = t;
            Object = o;
        }

        public readonly double T;
        public readonly Shape Object;

        public Computations PrepareComputations(Ray ray, IntersectionCollection xs = null)
        {
            var c = new Computations();
            c.T = T;
            c.Object = Object;
            c.Point = ray.Position(T);
            c.EyeVector = -ray.Direction;            
            c.NormalVector = Object.NormalAt(c.Point);

            if (c.NormalVector.Dot(c.EyeVector) < 0)
            {
                c.Inside = true;
                c.NormalVector = -c.NormalVector;
            }

            c.OverPoint = c.Point + c.NormalVector * Constants.Epsilon;
            c.ReflectV = ray.Direction.Reflect(c.NormalVector);

            xs = xs ?? new IntersectionCollection();
            HandleRefraction(c, xs);

            return c;
        }

        private void HandleRefraction(Computations comps, IntersectionCollection xs)
        {
            var containers = new List<Shape>();
            
            foreach (var intersection in xs.Intersections)
            {
                if (intersection.T == T)
                    comps.n1 = containers.Any()
                        ? containers.Last().Material.RefractiveIndex
                        : 1.0;

                if (containers.Contains(intersection.Object))
                    containers.Remove(intersection.Object);
                else
                    containers.Add(intersection.Object);

                if (intersection.T == T)
                {
                    comps.n2 = containers.Any()
                        ? containers.Last().Material.RefractiveIndex
                        : 1.0;
                    break;
                }
            }
        }
    }
}
