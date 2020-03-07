using System;

namespace TheRayTracerChallenge
{
    public class Computations
    {
        public double T { get; internal set; }
        public bool Inside { get; internal set; }
        public double n1 { get; internal set; }
        public double n2 { get; internal set; }        
        internal Tuple ReflectV { get; set; }
        internal Tuple OverPoint { get; set; }
        internal Tuple UnderPoint { get; set; }
        internal Shape Object { get; set; }
        internal Tuple Point { get; set; }
        internal Tuple EyeVector { get; set; }
        internal Tuple NormalVector { get; set; }

        /*
        Christophe Schlick, came up with an
        approximation to Fresnel’s equations that is much faster, and plenty accurate
        besides. Hurray for Schlick!         
         */
        internal double Schlick()
        {
            var cos = EyeVector.Dot(NormalVector);

            if (n1 > n2)
            {
                var n = n1 / n2;
                var sin2_t = n * n * (1.0 - cos * cos);
                if (sin2_t > 1)
                    return 1.0;
            }

            return 0;
        }
    }
}