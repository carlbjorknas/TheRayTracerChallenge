using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge
{
    class Cube : Shape
    {
        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            var (xtMin, xtMax) = CheckAxis(localRay.Point.x, localRay.Direction.x);
            var (ytMin, ytMax) = CheckAxis(localRay.Point.y, localRay.Direction.y);
            var (ztMin, ztMax) = CheckAxis(localRay.Point.z, localRay.Direction.z);

            var tMin = new[] { xtMin, ytMin, ztMin }.Max();
            var tMax = new[] { xtMax, ytMax, ztMax }.Min();

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

        public override Tuple LocalNormalAt(Tuple localPoint)
        {
            throw new NotImplementedException();
        }
    }
}
