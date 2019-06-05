using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    struct Tuple
    {
        double x, y, z, w;

        public Tuple(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public bool IsPoint => w == 1.0;
    }
}
