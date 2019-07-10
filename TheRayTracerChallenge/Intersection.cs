using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    internal struct Intersection
    {
        public Intersection(double t, Sphere o)
        {
            T = t;
            Object = o;
        }

        public readonly double T;
        public readonly Sphere Object;
    }
}
