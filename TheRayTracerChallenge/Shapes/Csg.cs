using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes.Utils;

namespace TheRayTracerChallenge.Shapes
{
    /// <summary>
    /// Constructive Solid Geometry
    /// </summary>
    class Csg : Shape
    {
        public Operation Operation { get; }
        public Shape Left { get; }
        public Shape Right { get; }

        public override Bounds Bounds => throw new NotImplementedException();

        public Csg(Operation operation, Shape left, Shape right)
        {
            Operation = operation;

            Left = left;
            left.Parent = this;

            Right = right;
            right.Parent = this;
        }

        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            throw new NotImplementedException();
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection? i = null)
        {
            throw new NotImplementedException();
        }
    }

    enum Operation
    {
        Union,
        Intersection,
        Difference
    }
}
