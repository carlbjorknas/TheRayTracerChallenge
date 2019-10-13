﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    class Plane : Shape
    {
        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            if (Math.Abs(localRay.Direction.y) < Constants.Epsilon)
            {
                return new IntersectionCollection();
            }

            var t = -localRay.Point.y / localRay.Direction.y;
            var i = new Intersection(t, this);
            return new IntersectionCollection(i);
        }

        public override Tuple LocalNormalAt(Tuple localPoint)
            => Tuple.Vector(0, 1, 0);
    }
}
