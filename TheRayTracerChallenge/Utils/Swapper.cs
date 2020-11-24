using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Utils
{
    static class Swapper
    {
        public static void Swap<T> (ref T x1, ref T x2) where T : struct
        {
            T temp = x1;
            x1 = x2;
            x2 = temp;
        }
    }
}
