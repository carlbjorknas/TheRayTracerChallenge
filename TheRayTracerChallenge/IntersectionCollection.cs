using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    public class IntersectionCollection
    {
        private readonly Intersection[] _intersections;

        public IntersectionCollection(params Intersection[] intersections)
        {
            _intersections = intersections;
        }

        public int Count 
            => _intersections.Length;
        public Intersection this[int index] 
            => _intersections[index];
    }
}
