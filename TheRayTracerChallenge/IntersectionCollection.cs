﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRayTracerChallenge
{
    internal class IntersectionCollection
    {
        public Intersection[] Intersections;

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
