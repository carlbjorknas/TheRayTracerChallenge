using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Patterns
{
    class CheckerPattern : Pattern
    {
        private readonly Color _col1;
        private readonly Color _col2;

        public CheckerPattern(Color col1, Color col2)
        {
            _col1 = col1;
            _col2 = col2;
        }

        public override Color ColorAt(Tuple point)
        {
            var sum =
                Math.Floor(point.x) +
                Math.Floor(point.y) +
                Math.Floor(point.z);

            return sum % 2 == 0 ? Color.White : Color.Black;
        }
    }
}
