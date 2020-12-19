using System;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Shapes
{
    class Sphere : Shape
    {
        public static Sphere UnitSphere()
            => new Sphere(Tuple.Point(0, 0, 0));

        public static Sphere Glass()
        {
            var s = UnitSphere();
            s.Material.Transparency = 1.0;
            s.Material.RefractiveIndex = RefractiveIndex.Glass;
            return s;
        }

        Guid Id;
        public Tuple Center;

        public Sphere(Tuple center)
        {
            Id = Guid.NewGuid();
            Center = center;            
        }

        public override IntersectionCollection LocalIntersect(Ray localRay)
        {            
            var sphereToRay = localRay.Origin - Center;
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

        public override Tuple LocalNormalAt(Tuple localPoint)
            => localPoint - Center;

        public override bool Equals(object obj)
            => obj is Sphere s &&
            s.Center.Equals(Center) &&
            s.Material.Equals(Material) &&
            s.Transform.Equals(Transform);

        public override int GetHashCode()
            => HashCode.Combine(Center.GetHashCode(), Material.GetHashCode(), Transform.GetHashCode());
    }
}
