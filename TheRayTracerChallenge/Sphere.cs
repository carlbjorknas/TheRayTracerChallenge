﻿using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Spatial.Euclidean;

namespace TheRayTracerChallenge
{
    class Sphere : Shape
    {
        public static Sphere UnitSphere()
            => new Sphere(Tuple.Point(0, 0, 0));

        Guid Id;
        public Tuple Center;

        public Sphere(Tuple center)
        {
            Id = Guid.NewGuid();
            Center = center;            
        }

        protected override IntersectionCollection LocalIntersect(Ray localRay)
        {            
            var sphereToRay = localRay.Point - Center;
            var a = localRay.Direction.Dot(localRay.Direction);
            var b = 2 * (localRay.Direction.Dot(sphereToRay));
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
            var objectPoint = Transform.Inverse.Transform(point);
            var objectNormal = objectPoint - Center;
            var worldNormal = Transform.Inverse.Transpose.Transform(objectNormal);
            worldNormal.w = 0;
            return worldNormal.Normalize;
        }

        public override bool Equals(object obj)
            => obj is Sphere s &&
            s.Center.Equals(Center) &&
            s.Material.Equals(Material) &&
            s.Transform.Equals(Transform);

        public override int GetHashCode()
            => HashCode.Combine(Center.GetHashCode(), Material.GetHashCode(), Transform.GetHashCode());
    }
}
