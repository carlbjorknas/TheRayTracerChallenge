using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes;

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
            => ColorAtShapePoint(shape.WorldToObject(worldPoint));
        
        public Color ColorAtShapePoint(Tuple shapePoint)
            => ColorAt(Transform.Inverse.Transform(shapePoint));      
    }
}
