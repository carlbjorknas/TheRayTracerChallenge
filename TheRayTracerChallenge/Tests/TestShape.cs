using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    class TestShape : Shape
    {
        public Ray LocalRay { get; private set; }

        protected override IntersectionCollection LocalIntersect(Ray localRay)
        {
            LocalRay = localRay;
            return new IntersectionCollection();
        }

        protected override Tuple LocalNormalAt(Tuple localPoint)
            => Tuple.Vector(localPoint.x, localPoint.y, localPoint.z);
    }
}
