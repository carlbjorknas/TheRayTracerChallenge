using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Utils
{
    static class C
    {
        public static double Epsilon => 0.00001;
        public static double SqrtOf2 => Math.Sqrt(2);
        public static double SqrtOf2DividedBy2 => SqrtOf2 / 2;
    }

    static class RefractiveIndex
    {
        public static double Vacuum = 1;
        public static double Air = 1.00029;
        public static double Water = 1.333;
        public static double Glass = 1.5;
        public static double Diamond = 2.417;
    }
}
