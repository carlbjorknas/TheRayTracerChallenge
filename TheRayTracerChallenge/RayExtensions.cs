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

        public static double[] Intersect(this Ray3D ray, Sphere sphere)
        {
            var sphereToRay = ray.ThroughPoint - sphere.Center;
            var a = ray.Direction.DotProduct(ray.Direction);
            var b = 2 * (ray.Direction.DotProduct(sphereToRay));
            var c = sphereToRay.DotProduct(sphereToRay) - 1;
            var discriminant = b * b - 4 * a * c;

            var intersects = discriminant >= 0;
            if (!intersects)
            {
                return new double[] { };
            }

            var t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            var t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

            return new[] { t1, t2 };
        }
    }
}
