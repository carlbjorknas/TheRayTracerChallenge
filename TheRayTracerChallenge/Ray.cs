using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    struct Ray
    {
        public Ray(Tuple origin, Tuple direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Tuple Origin { get; }
        public Tuple Direction { get; }

        public Tuple Position(double t)
            => Origin + Direction * t;
        
        public Ray Transform(Transformation transformation)
            => new Ray(
                transformation.Transform(Origin),
                transformation.Transform(Direction));
    }
}
