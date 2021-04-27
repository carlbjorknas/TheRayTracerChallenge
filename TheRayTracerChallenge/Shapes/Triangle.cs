using System;
using System.Collections.Generic;
using System.Linq;
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

        public override Bounds Bounds
            => new Bounds(
                Tuple.Point(
                    new[] { P1.x, P2.x, P3.x }.Min(),
                    new[] { P1.y, P2.y, P3.y }.Min(),
                    new[] { P1.z, P2.z, P3.z }.Min()),
                Tuple.Point(
                    new[] { P1.x, P2.x, P3.x }.Max(),
                    new[] { P1.y, P2.y, P3.y }.Max(),
                    new[] { P1.z, P2.z, P3.z }.Max()));

        public override IntersectionCollection LocalIntersect(Ray localRay)
            => LocalIntersect(localRay, (t, _, __) => new Intersection(t, this));

        protected IntersectionCollection LocalIntersect(Ray localRay, Func<double, double, double, Intersection> newIntersection)
        {
            var dirCrossE2 = localRay.Direction.Cross(EdgeVec2);
            var det = EdgeVec1.Dot(dirCrossE2);
            if (Math.Abs(det) < C.Epsilon)
                return new IntersectionCollection();

            var f = 1 / det;
            var p1ToOrigin = localRay.Origin - P1;
            var u = f * p1ToOrigin.Dot(dirCrossE2);
            if (u < 0 || u > 1)
                return new IntersectionCollection();

            var originCrossE1 = p1ToOrigin.Cross(EdgeVec1);
            var v = f * localRay.Direction.Dot(originCrossE1);
            if (v < 0 || u + v > 1)
                return new IntersectionCollection();

            var t = f * EdgeVec2.Dot(originCrossE1);
            return new IntersectionCollection(newIntersection(t, u, v));
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection? i = null)
        {
            return Normal;
        }
    }
}
