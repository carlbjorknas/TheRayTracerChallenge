using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Patterns
{
    class SolidPattern : Pattern
    {
        private readonly Color _color;

        public SolidPattern(Color color)
        {
            _color = color;
        }

        public override Color ColorAt(Tuple point)
            => _color;
    }
}
