using NUnit.Framework.Constraints;
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
        public CsgOperation Operation { get; }
        public Shape Left { get; }
        public Shape Right { get; }

        public override Bounds Bounds => throw new NotImplementedException();

        public Csg(CsgOperation operation, Shape left, Shape right)
        {
            Operation = operation;

            Left = left;
            left.Parent = this;

            Right = right;
            right.Parent = this;
        }

        internal static bool IntersectionAllowed(CsgOperation op, CsgOperand hitShape, bool insideLeft, bool insideRight)
        {
            if (op == CsgOperation.Union)
            {
                return
                    (hitShape == CsgOperand.Left && !insideRight) ||
                    (hitShape == CsgOperand.Right && !insideLeft);
            }

            return false;
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

    public enum CsgOperation
    {
        Union,
        Intersection,
        Difference
    }

    public enum CsgOperand
    {
        Left,
        Right
    }
}
