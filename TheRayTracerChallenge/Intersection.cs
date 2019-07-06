using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    public struct Intersection
    {
        public Intersection(double t, object o)
        {
            T = t;
            Object = o;
        }

        public readonly double T;
        public readonly object Object;
    }
}
