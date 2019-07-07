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

        public readonly double x, y, z;
        public double w;

        public Tuple(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public bool IsPoint => w == 1.0;

        public bool IsVector => w == 0.0;

        public double Magnitude => 
            Math.Sqrt(
                Math.Pow(x, 2) + 
                Math.Pow(y, 2) +
                Math.Pow(z, 2) +
                Math.Pow(w, 2));

        public Tuple Normalize => this / Magnitude;
        
        public override bool Equals(object obj)
        {
            return
                obj is Tuple t &&
                Equals(t.x, x) &&
                Equals(t.y, y) &&
                Equals(t.z, z) &&
                Equals(t.w, w);
        }

        private bool Equals(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < 0.00001;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z, w);
        }

        public static Tuple operator+(Tuple t1, Tuple t2)
        {
            return new Tuple(t1.x + t2.x, t1.y + t2.y, t1.z + t2.z, t1.w + t2.w);
        }

        public static Tuple operator-(Tuple t1, Tuple t2)
        {
            return new Tuple(t1.x - t2.x, t1.y - t2.y, t1.z - t2.z, t1.w - t2.w);
        }

        public static Tuple operator-(Tuple t)
        {
            return new Tuple(-t.x, -t.y, -t.z, -t.w);
        }

        public static Tuple operator*(Tuple t, double d)
        {
            return new Tuple(d*t.x, d*t.y, d*t.z, d*t.w);
        }

        public double Dot(Tuple t) => 
            x * t.x + 
            y * t.y + 
            z * t.z + 
            w * t.w;
        
        public Tuple Cross(Tuple t)
        => Vector(
            y * t.z - z * t.y,
            z * t.x - x * t.z,
            x * t.y - y * t.x);

        public static Tuple operator/(Tuple t, double d)
        {
            return new Tuple(t.x / d, t.y / d, t.z / d, t.w / d);
        }
    }
}
