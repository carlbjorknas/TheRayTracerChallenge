using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Spatial.Euclidean;

namespace TheRayTracerChallenge
{
    public struct Sphere
    {
        public static Sphere UnitSphere()
            => new Sphere(new Point3D(0, 0, 0));

        public Sphere(Point3D center)
        {
            Id = Guid.NewGuid();
            Center = center;
        }

        Guid Id;
        public Point3D Center;       
    }
}
