using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Patterns
{
    class BlendedPattern : Pattern
    {
        private readonly Pattern _pattern1;
        private readonly Pattern _pattern2;

        public BlendedPattern(Pattern pattern1, Pattern pattern2)
        {
            _pattern1 = pattern1;
            _pattern2 = pattern2;
        }

        public override Color ColorAt(Tuple point)
        {
            var col1 = _pattern1.ColorAtShapePoint(point);
            var col2 = _pattern2.ColorAtShapePoint(point);
            return (col1 + col2) / 2;
        }
    }
}
