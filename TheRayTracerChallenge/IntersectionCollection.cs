using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge
{
    internal class IntersectionCollection
    {
        public Intersection[] Intersections;

        public static IntersectionCollection Create(params (double T, Shape shape)[] intersections)
        {
            return new IntersectionCollection(intersections.Select(x => new Intersection(x.T, x.shape)).ToArray());
        }

        public static IntersectionCollection CreateFromCollections(params IntersectionCollection[] xs)
        {
            return new IntersectionCollection(xs.SelectMany(x => x.Intersections).ToArray());
        }

        public IntersectionCollection(params Intersection[] intersections)
        {
            Intersections = intersections
                .OrderBy(i => i.T)
                .ToArray();
        }

        public IntersectionCollection(IEnumerable<Intersection> intersections)
        {
            Intersections = intersections.ToArray();
        }

        public int Count 
            => Intersections.Length;
        public Intersection this[int index] 
            => Intersections[index];

        internal Intersection? Hit()
        {
            return Intersections
                .Where(i => i.T >= 0)
                .Cast<Intersection?>()
                .FirstOrDefault();
        }
    }
}
