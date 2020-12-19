using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Shapes
{
    class Cylinder : Shape
    {
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

            return new IntersectionCollection(new Intersection(1, this));
        }

        public override Tuple LocalNormalAt(Tuple localPoint)
        {
            throw new NotImplementedException();
        }
    }
}
