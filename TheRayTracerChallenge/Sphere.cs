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

        Guid Id;
        public Point3D Center;

        public Sphere(Point3D center)
        {
            Id = Guid.NewGuid();
            Center = center;
        }

        public IntersectionCollection Intersect(Ray3D ray)
        {
            var sphereToRay = ray.ThroughPoint - Center;
            var a = ray.Direction.DotProduct(ray.Direction);
            var b = 2 * (ray.Direction.DotProduct(sphereToRay));
            var c = sphereToRay.DotProduct(sphereToRay) - 1;
            var discriminant = b * b - 4 * a * c;

            var intersects = discriminant >= 0;
            if (!intersects)
            {
                return new IntersectionCollection();
            }

            var t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            var t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

            var i1 = new Intersection(Math.Min(t1, t2), this);
            var i2 = new Intersection(Math.Max(t1, t2), this);

            return new IntersectionCollection(i1, i2);
        }
    }
}
