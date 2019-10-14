using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    class TestPattern : Pattern
    {
        public override Color ColorAt(Tuple point)
            => new Color(point.x, point.y, point.z);
    }
}
