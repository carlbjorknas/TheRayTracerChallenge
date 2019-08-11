using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    internal class PointLight
    {
        public Tuple Position;
        public Color Intensity;

        public PointLight(Tuple position, Color intensity)
        {
            this.Position = position;
            this.Intensity = intensity;
        }

        public override bool Equals(object obj)
        {
            return obj is PointLight pl &&
                pl.Intensity.Equals(Intensity) &&
                pl.Position.Equals(Position);
        }

        public override int GetHashCode()
            => HashCode.Combine(Position.GetHashCode(), Intensity.GetHashCode());
    }
}
