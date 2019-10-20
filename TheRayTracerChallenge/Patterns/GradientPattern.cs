using System;

namespace TheRayTracerChallenge.Patterns
{
    internal class GradientPattern : Pattern
    {
        private Color _col1;
        private Color _colorDistance;

        public GradientPattern(Color col1, Color col2)
        {
            this._col1 = col1;
            _colorDistance = col2 - col1;
        }

        public override Color ColorAt(Tuple point)
        {            
            var fraction = point.x - Math.Floor(point.x);
            return _col1 + _colorDistance * fraction;
        }
    }
}