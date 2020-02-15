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
    }
}