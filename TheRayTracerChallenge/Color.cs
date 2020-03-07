using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge
{
    struct Color
    {
        public double r, g, b;

        public Color(double r, double g, double b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public static Color operator +(Color c1, Color c2)
        {
            return new Color(c1.r + c2.r, c1.g + c2.g, c1.b + c2.b);
        }

        public static Color operator -(Color c1, Color c2)
        {
            return new Color(c1.r - c2.r, c1.g - c2.g, c1.b - c2.b);
        }

        public static Color operator *(Color c, double d)
        {
            return new Color(d * c.r, d * c.g, d * c.b);
        }

        public static Color operator *(Color c1, Color c2)
        {
            return new Color(c1.r * c2.r, c1.g * c2.g, c1.b * c2.b);
        }

        public static Color operator /(Color c, double d)
            => new Color(c.r / d, c.g / d, c.b / d);

        public override bool Equals(object obj)
        {
            return
                obj is Color t &&
                Equals(t.r, r) &&
                Equals(t.g, g) &&
                Equals(t.b, b);
        }

        private bool Equals(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < C.Epsilon;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(r, g, b);
        }

        public override string ToString()
            => $"{base.ToString()} r={r}, g={g}, b={b}";
    
        public static Color Black { get; } = new Color();
        public static Color White { get; } = new Color(1, 1, 1);
    }
}
