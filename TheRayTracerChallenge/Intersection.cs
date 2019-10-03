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

        public Computations PrepareComputations(Ray ray)
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

            return c;
        }
    }
}
