using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    struct Tuple
    {
        public static Tuple Point(double x, double y, double z)
        {
            return new Tuple(x, y, z, 1);
        }

        public static Tuple Vector(double x, double y, double z)
        {
            return new Tuple(x, y, z, 0);
        }

        readonly double x, y, z, w;

        public Tuple(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public bool IsPoint => w == 1.0;

        public bool IsVector => w == 0.0;

        public override bool Equals(object obj)
        {
            return
                obj is Tuple t &&
                t.x == x &&
                t.y == y &&
                t.z == z &&
                t.w == w;                        
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z, w);
        }
    }
}
