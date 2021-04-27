using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheRayTracerChallenge.Shapes.Utils;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Shapes
{
    class Cube : Shape
    {
        public override Bounds Bounds
            => new Bounds(Tuple.Point(-1, -1, -1), Tuple.Point(1, 1, 1));

        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            var (xtMin, xtMax) = CheckAxis(localRay.Origin.x, localRay.Direction.x);
            var (ytMin, ytMax) = CheckAxis(localRay.Origin.y, localRay.Direction.y);
            var (ztMin, ztMax) = CheckAxis(localRay.Origin.z, localRay.Direction.z);

            var tMin = new[] { xtMin, ytMin, ztMin }.Max();
            var tMax = new[] { xtMax, ytMax, ztMax }.Min();

            if (tMin > tMax)
                return new IntersectionCollection();

            return new IntersectionCollection(
                new Intersection(tMin, this), 
                new Intersection(tMax, this));
        }

        private (double tMin, double tMax) CheckAxis(double origin, double direction)
        {
            var tMinNumerator = -1 - origin;
            var tMaxNumerator = 1 - origin;

            double tMin, tMax;
            if (Math.Abs(direction) >= C.Epsilon)
            {
                tMin = tMinNumerator / direction;
                tMax = tMaxNumerator / direction;
            }
            else
            {
                tMin = tMinNumerator * double.PositiveInfinity;
                tMax = tMaxNumerator * double.PositiveInfinity;
            }

            if (tMin > tMax)
                Swapper.Swap(ref tMin, ref tMax);

            return (tMin, tMax);
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection? i = null)
        {
            var max = new[] { Math.Abs(localPoint.x), Math.Abs(localPoint.y), Math.Abs(localPoint.z) }.Max();

            if (max == Math.Abs(localPoint.x))
                return Tuple.Vector(localPoint.x, 0, 0);
            if (max == Math.Abs(localPoint.y))
                return Tuple.Vector(0, localPoint.y, 0);
            return Tuple.Vector(0, 0, localPoint.z);
        }
    }
}
