using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheRayTracerChallenge.Tests.Shapes;

namespace TheRayTracerChallenge.Shapes
{
    class Group : Shape
    {
        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            var xs = Shapes
                .SelectMany(shape => shape.Intersect(localRay).Intersections)
                .OrderBy(intersection => intersection.T);
            return new IntersectionCollection(xs);
        }

        public override Tuple LocalNormalAt(Tuple localPoint)
        {
            throw new NotImplementedException();
        }

        public List<Shape> Shapes { get; } = new List<Shape>();

        internal void AddChild(Shape shape)
        {
            Shapes.Add(shape);
            shape.Parent = this;
        }
    }
}
