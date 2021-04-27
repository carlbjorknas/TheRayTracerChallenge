using System;
using System.Collections.Generic;
using TheRayTracerChallenge.Shapes.Utils;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Shapes
{
    class Cylinder : Shape
    {
        public Cylinder()
        {
        }

        public Cylinder(double min, double max, bool closed = false)
        {
            Min = min;
            Max = max;
            Closed = closed;
        }

        public double Min { get; internal set; } = double.NegativeInfinity;
        public double Max { get; internal set; } = double.PositiveInfinity;
        public bool Closed { get; internal set; }

        public override Bounds Bounds
            => new Bounds(Tuple.Point(-1, Min, -1), Tuple.Point(1, Max, 1));

        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            var xs = new List<Intersection>();
            xs.AddRange(IntersectWalls(localRay));
            xs.AddRange(IntersectCaps(localRay));

            return new IntersectionCollection(xs);
        }

        private IEnumerable<Intersection> IntersectWalls(Ray localRay)
        {
            var a = Math.Pow(localRay.Direction.x, 2) + Math.Pow(localRay.Direction.z, 2);

            if (a < C.Epsilon)
                // Ray is parallell to the y axis
                yield break;

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
                yield break;

            var t0 = (-b - Math.Sqrt(disc)) / (2 * a);
            var t1 = (-b + Math.Sqrt(disc)) / (2 * a);

            if (t0 > t1)
                Swapper.Swap(ref t0, ref t1);

            var y0 = localRay.Origin.y + t0 * localRay.Direction.y;
            if (Min < y0 && y0 < Max)
                yield return new Intersection(t0, this);

            var y1 = localRay.Origin.y + t1 * localRay.Direction.y;
            if (Min < y1 && y1 < Max)
                yield return new Intersection(t1, this);
        }

        private IEnumerable<Intersection> IntersectCaps(Ray ray)
        {
            // Caps only matter if the cylinder is closed and might possibly be
            // intersected by the ray.
            if (!Closed || Math.Abs(ray.Direction.y) < C.Epsilon)
                yield break;

            // Check for an intersection with the lower end cap by intersecting
            // the ray with the plane at y=Min
            var t = (Min - ray.Origin.y) / ray.Direction.y;
            if (CheckCap(ray, t))
                yield return new Intersection(t, this);

            // Check for an intersection with the upper end cap by intersecting
            // the ray with the plane at y=Max
            t = (Max - ray.Origin.y) / ray.Direction.y;
            if (CheckCap(ray, t))
                yield return new Intersection(t, this);
        }

        private bool CheckCap(Ray ray, double t)
        {
            var x = ray.Origin.x + t * ray.Direction.x;
            var z = ray.Origin.z + t * ray.Direction.z;

            return Math.Pow(x, 2) + Math.Pow(z, 2) <= 1;
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection? i = null)
        {
            var distanceFromYAxis = Math.Pow(localPoint.x, 2) + Math.Pow(localPoint.z, 2);

            if (distanceFromYAxis < 1)
            {
                if (localPoint.y >= Max - C.Epsilon)
                    return Tuple.Vector(0, 1, 0);
                if (localPoint.y <= Min + C.Epsilon)
                    return Tuple.Vector(0, -1, 0);
            }

            return Tuple.Vector(localPoint.x, 0, localPoint.z);
        }
    }
}
