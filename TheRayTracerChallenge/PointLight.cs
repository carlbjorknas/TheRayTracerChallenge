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
    }
}
