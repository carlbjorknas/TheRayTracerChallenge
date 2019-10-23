using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Patterns
{
    abstract class Pattern
    {
        protected Pattern()
        {
            Transform = Transformation.Identity;
        }

        public Transformation Transform { get; set; }

        public abstract Color ColorAt(Tuple point);

        public Color PatternColorAtShape(Shape shape, Tuple worldPoint)
            => ColorAtShapePoint(shape.Transform.Inverse.Transform(worldPoint));
        
        public Color ColorAtShapePoint(Tuple shapePoint)
            => ColorAt(Transform.Inverse.Transform(shapePoint));      
    }
}
