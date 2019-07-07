using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    struct Ray
    {
        public Ray(Tuple point, Tuple direction)
        {
            Point = point;
            Direction = direction;
        }

        public Tuple Point { get; }
        public Tuple Direction { get; }

        public Tuple Position(double t)
            => Point + Direction * t;
        
        public Ray Transform(Transformation transformation)
            => new Ray(
                transformation.Transform(Point),
                transformation.Transform(Direction));
    }
}
