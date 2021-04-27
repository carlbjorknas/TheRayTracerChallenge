using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes.Utils;

namespace TheRayTracerChallenge.Shapes
{
    class SmoothTriangle : Triangle
    {
        public SmoothTriangle(Tuple p1, Tuple p2, Tuple p3, Tuple n1, Tuple n2, Tuple n3) : base(p1, p2, p3)
        {
            N1 = n1;
            N2 = n2;
            N3 = n3;
        }

        public Tuple N1 { get; }
        public Tuple N2 { get; }
        public Tuple N3 { get; }

        public override Bounds Bounds => throw new NotImplementedException();

        public override IntersectionCollection LocalIntersect(Ray localRay)
            => LocalIntersect(localRay, (t, u, v) => new Intersection(t, this, u, v));

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection? hit)
        {
            return 
                N2 * hit.Value.U + 
                N3 * hit.Value.V + 
                N1 * (1 - hit.Value.U - hit.Value.V);
        }
    }
}
