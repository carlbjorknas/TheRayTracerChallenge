using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    class StripePattern : Pattern
    {
        public StripePattern(Color color1, Color color2)
        {
            Color1 = color1;
            Color2 = color2;            
        }

        public Color Color1 { get; }
        public Color Color2 { get; }        

        public override Color ColorAt(Tuple point)
            => Math.Floor(point.x) % 2 == 0 
                ? Color1 
                : Color2;
    }
}
