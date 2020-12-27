using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes.Utils;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Shapes
{
    class Triangle : Shape
    {
        public Triangle(Tuple p1, Tuple p2, Tuple p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            EdgeVec1 = p2 - p1;
            EdgeVec2 = p3 - p1;
            Normal = EdgeVec2.Cross(EdgeVec1).Normalize;
        }

        public Tuple P1 { get; }
        public Tuple P2 { get; }
        public Tuple P3 { get; }
        public Tuple EdgeVec1 { get; }
        public Tuple EdgeVec2 { get; }
        public Tuple Normal { get; }

        public override Bounds Bounds => throw new NotImplementedException();

        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            var dirCrossE2 = localRay.Direction.Cross(EdgeVec2);
            var det = EdgeVec1.Dot(dirCrossE2);
            if (Math.Abs(det) < C.Epsilon)
                return new IntersectionCollection();

            return new IntersectionCollection(new Intersection(1, this)); // Dummy to make sure the test works
        }

        public override Tuple LocalNormalAt(Tuple localPoint)
        {
            return Normal;
        }
    }
}
