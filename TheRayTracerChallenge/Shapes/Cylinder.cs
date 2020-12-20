using System;
using System.Collections.Generic;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Shapes
{
    class Cylinder : Shape
    {
        public Cylinder()
        {
        }

        public Cylinder(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public double Min { get; internal set; } = double.NegativeInfinity;
        public double Max { get; internal set; } = double.PositiveInfinity;

        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            var a = Math.Pow(localRay.Direction.x, 2) + Math.Pow(localRay.Direction.z, 2);
            
            if (a < C.Epsilon)
                // Ray is parallell to the y axis
                return new IntersectionCollection();

            var b = 
                2 * localRay.Origin.x * localRay.Direction.x +
                2 * localRay.Origin.z * localRay.Direction.z;

            var c = 
                Math.Pow(localRay.Origin.x, 2) +
                Math.Pow(localRay.Origin.z, 2) - 
                1;

            var disc = Math.Pow(b, 2) - 4 * a * c;

            if (disc < 0)
                // Ray does not intersect the cylinder
                return new IntersectionCollection();

            var t0 = (-b - Math.Sqrt(disc)) / (2 * a);
            var t1 = (-b + Math.Sqrt(disc)) / (2 * a);

            if (t0 > t1)
                Swapper.Swap(ref t0, ref t1);

            var xs = new List<Intersection>();

            var y0 = localRay.Origin.y + t0 * localRay.Direction.y;
            if (Min < y0 && y0 < Max)
                xs.Add(new Intersection(t0, this));

            var y1 = localRay.Origin.y + t1 * localRay.Direction.y;
            if (Min < y1 && y1 < Max)
                xs.Add(new Intersection(t1, this));

            return new IntersectionCollection(xs);
        }

        public override Tuple LocalNormalAt(Tuple localPoint)
        {
            return Tuple.Vector(localPoint.x, 0, localPoint.z);
        }
    }
}
