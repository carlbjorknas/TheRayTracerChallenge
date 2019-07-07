using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Spatial.Euclidean;

namespace TheRayTracerChallenge
{
    struct Sphere
    {
        public static Sphere UnitSphere()
            => new Sphere(Tuple.Point(0, 0, 0));

        Guid Id;
        public Tuple Center;

        public Sphere(Tuple center)
        {
            Id = Guid.NewGuid();
            Center = center;
            Transform = Transformation.Identity;
        }

        public Transformation Transform { get; set; }

        public IntersectionCollection Intersect(Ray ray)
        {
            var transformedRay = ray.Transform(Transform.Inverse);
            var sphereToRay = transformedRay.Point - Center;
            var a = transformedRay.Direction.Dot(transformedRay.Direction);
            var b = 2 * (transformedRay.Direction.Dot(sphereToRay));
            var c = sphereToRay.Dot(sphereToRay) - 1;
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

        internal Tuple NormalAt(Tuple point)
        {
            return (point - Center).Normalize;
        }
    }
}
