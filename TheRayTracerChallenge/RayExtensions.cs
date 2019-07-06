using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    public static class RayExtensions
    {
        public static Point3D Position(this Ray3D ray, double t)
        {
            return ray.ThroughPoint + ray.Direction.ScaleBy(t);
        }
    }
}
