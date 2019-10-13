namespace TheRayTracerChallenge
{
    public class Computations
    {
        public double T { get; internal set; }
        public bool Inside { get; internal set; }
        internal Tuple OverPoint { get; set; }
        internal Shape Object { get; set; }
        internal Tuple Point { get; set; }
        internal Tuple EyeVector { get; set; }
        internal Tuple NormalVector { get; set; }
    }
}