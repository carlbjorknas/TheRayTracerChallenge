using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Patterns
{
    class CheckerPattern : Pattern
    {
        private readonly Pattern _pattern1;
        private readonly Pattern _pattern2;

        public CheckerPattern(Color col1, Color col2) 
            : this(new SolidPattern(col1), new SolidPattern(col2))
        {
        }

        public CheckerPattern(Pattern pattern1, Pattern pattern2)
        {
            _pattern1 = pattern1;
            _pattern2 = pattern2;
        }

        public override Color ColorAt(Tuple point)
        {
            var sum =
                Math.Floor(point.x) +
                Math.Floor(point.y) +
                Math.Floor(point.z);

            return sum % 2 == 0 
                ? _pattern1.ColorAtShapePoint(point)
                : _pattern2.ColorAtShapePoint(point);
        }
    }
}
