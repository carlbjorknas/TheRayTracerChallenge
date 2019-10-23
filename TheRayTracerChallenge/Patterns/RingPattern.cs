using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Patterns
{
    class RingPattern : Pattern
    {
        private readonly Pattern _pattern1;
        private readonly Pattern _pattern2;

        public RingPattern(Color col1, Color col2) 
            : this(new SolidPattern(col1), new SolidPattern(col2))
        {
        }

        public RingPattern(Pattern pattern1, Pattern pattern2)
        {
            _pattern1 = pattern1;
            _pattern2 = pattern2;
        }

        public override Color ColorAt(Tuple point)
        {
            var distance = Math.Floor(
                Math.Sqrt(
                     Math.Pow(point.x, 2) +
                     Math.Pow(point.z, 2)));

            return distance % 2 == 0 
                ? _pattern1.ColorAtShapePoint(point) 
                : _pattern2.ColorAtShapePoint(point);
        }
    }
}
