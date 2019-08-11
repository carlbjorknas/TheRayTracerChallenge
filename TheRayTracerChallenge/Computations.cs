﻿namespace TheRayTracerChallenge
{
    public class Computations
    {
        public double T { get; internal set; }
        public bool Inside { get; internal set; }
        internal Sphere Object { get; set; }
        internal Tuple Point { get; set; }
        internal Tuple EyeVector { get; set; }
        internal Tuple NormalVector { get; set; }
    }
}