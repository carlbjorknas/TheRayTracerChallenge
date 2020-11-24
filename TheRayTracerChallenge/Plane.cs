using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge
{
    class Plane : Shape
    {
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
