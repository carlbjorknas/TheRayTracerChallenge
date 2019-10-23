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

            var pattern1point = _pattern1.Transform.Inverse.Transform(point);
            var pattern2point = _pattern2.Transform.Inverse.Transform(point);
            return distance % 2 == 0 ? _pattern1.ColorAt(pattern1point) : _pattern2.ColorAt(pattern2point);
        }
    }
}
