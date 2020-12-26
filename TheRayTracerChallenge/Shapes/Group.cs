using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Shapes
{
    class Group : Shape
    {
        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            throw new NotImplementedException();
        }

        public override Tuple LocalNormalAt(Tuple localPoint)
        {
            throw new NotImplementedException();
        }

        public List<Shape> Shapes { get; } = new List<Shape>();
    }
}
