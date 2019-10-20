using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Patterns
{
    class RadialGradientPattern : Pattern
    {
        private readonly Color _col1;
        private readonly Color _col2;
        private readonly Color _colorDistance;


        public RadialGradientPattern(Color col1, Color col2)
        {
            _col1 = col1;
            _colorDistance = col2 - col1;
        }

        public override Color ColorAt(Tuple point)
        {
            var distance = 
                Math.Sqrt(
                    Math.Pow(point.x, 2) +
                    Math.Pow(point.z, 2));

            var fraction = distance - Math.Floor(distance);
            return _col1 + _colorDistance * fraction;
        }
    }
}
