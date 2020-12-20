using System;
using System.Collections.Generic;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Shapes
{
    class Cone : Shape
    {
        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            var xs = new List<Intersection>();
            xs.AddRange(IntersectWalls(localRay));
            //xs.AddRange(IntersectCaps(localRay));

            return new IntersectionCollection(xs);
        }

        private IEnumerable<Intersection> IntersectWalls(Ray localRay)
        {
            var a = Math.Pow(localRay.Direction.x, 2) - Math.Pow(localRay.Direction.y, 2) + Math.Pow(localRay.Direction.z, 2);

            var b =
                2 * localRay.Origin.x * localRay.Direction.x -
                2 * localRay.Origin.y * localRay.Direction.y +
                2 * localRay.Origin.z * localRay.Direction.z;

            var c =
                Math.Pow(localRay.Origin.x, 2) -
                Math.Pow(localRay.Origin.y, 2) +
                Math.Pow(localRay.Origin.z, 2);

            if (a < C.Epsilon)
            {
                // Ray is parallell to one of the cones halves
                if (b < C.Epsilon)
                    yield break;
                var t = -c / 2 * b;
                yield return new Intersection(t, this);
            }

            var disc = Math.Pow(b, 2) - 4 * a * c;

            if (disc < 0)
                // Ray does not intersect the cone
                yield break;

            var t0 = (-b - Math.Sqrt(disc)) / (2 * a);
            var t1 = (-b + Math.Sqrt(disc)) / (2 * a);

            if (t0 > t1)
                Swapper.Swap(ref t0, ref t1);

            //var y0 = localRay.Origin.y + t0 * localRay.Direction.y;
            //if (Min < y0 && y0 < Max)
                yield return new Intersection(t0, this);

            //var y1 = localRay.Origin.y + t1 * localRay.Direction.y;
            //if (Min < y1 && y1 < Max)
                yield return new Intersection(t1, this);
        }

        public override Tuple LocalNormalAt(Tuple localPoint)
        {
            throw new NotImplementedException();
        }
    }
}
