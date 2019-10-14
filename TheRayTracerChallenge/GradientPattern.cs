using System;

namespace TheRayTracerChallenge
{
    internal class GradientPattern : Pattern
    {
        private Color _col1;
        private Color _col2;

        public GradientPattern(Color col1, Color col2)
        {
            this._col1 = col1;
            this._col2 = col2;
        }

        public override Color ColorAt(Tuple point)
        {
            var distance = _col2 - _col1;
            var fraction = point.x - Math.Floor(point.x);
            return _col1 + distance * fraction;
        }
    }
}