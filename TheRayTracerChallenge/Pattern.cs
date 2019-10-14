using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    abstract class Pattern
    {
        protected Pattern()
        {
            Transform = Transformation.Identity;
        }

        public Transformation Transform { get; set; }

        public abstract Color ColorAt(Tuple point);

        public Color PatternColorAtShape(Shape shape, Tuple point)
        {
            var shapePoint = shape.Transform.Inverse.Transform(point);
            var patternPoint = Transform.Inverse.Transform(shapePoint);
            return ColorAt(patternPoint);
        }
    }
}
