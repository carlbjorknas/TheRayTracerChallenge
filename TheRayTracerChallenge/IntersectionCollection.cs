using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRayTracerChallenge
{
    internal class IntersectionCollection
    {
        private readonly Intersection[] _intersections;

        public IntersectionCollection(params Intersection[] intersections)
        {
            _intersections = intersections
                .OrderBy(i => i.T)
                .ToArray();
        }

        public int Count 
            => _intersections.Length;
        public Intersection this[int index] 
            => _intersections[index];

        internal Intersection? Hit()
        {
            return _intersections
                .Where(i => i.T >= 0)
                .Cast<Intersection?>()
                .FirstOrDefault();
        }
    }
}
