using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes.Utils;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Shapes
{
    class Plane : Shape
    {
        public override Bounds Bounds
            => new Bounds(
                Tuple.Point(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity),
                Tuple.Point(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity));

        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            if (Math.Abs(localRay.Direction.y) < C.Epsilon)
            {
                return new IntersectionCollection();
            }

            var t = -localRay.Origin.y / localRay.Direction.y;
            var i = new Intersection(t, this);
            return new IntersectionCollection(i);
        }

        public override Tuple LocalNormalAt(Tuple localPoint)
            => Tuple.Vector(0, 1, 0);
    }
}
