using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Patterns
{
    class RingPattern : Pattern
    {
        private Color _col1;
        private Color _col2;

        public RingPattern(Color col1, Color col2)
        {
            _col1 = col1;
            _col2 = col2;
        }

        public override Color ColorAt(Tuple point)
        {
            var distance = Math.Floor(
                Math.Sqrt(
                     Math.Pow(point.x, 2) +
                     Math.Pow(point.z, 2)));

            return distance % 2 == 0 ? _col1 : _col2;
        }
    }
}
