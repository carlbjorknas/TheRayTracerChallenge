using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Patterns;

namespace TheRayTracerChallenge.Tests.Patterns
{
    class TestPattern : Pattern
    {
        public override Color ColorAt(Tuple point)
            => new Color(point.x, point.y, point.z);
    }
}
