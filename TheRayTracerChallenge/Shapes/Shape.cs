using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Shapes
{
    abstract class Shape
    {
        protected Shape()
        {
            Transform = Transformation.Identity;
            Material = new Material();
        }

        public Transformation Transform { get; set; }
        public Material Material { get; set; }

        public Group Parent { get; set; }

        public IntersectionCollection Intersect(Ray ray)
        {
            var localRay = ray.Transform(Transform.Inverse);
            return LocalIntersect(localRay);
        }

        public abstract IntersectionCollection LocalIntersect(Ray localRay);

        public Tuple NormalAt(Tuple point)
        {
            var localPoint = WorldToObject(point);
            var localNormal = LocalNormalAt(localPoint);
            return NormalToWorld(localNormal);
        }

        public abstract Tuple LocalNormalAt(Tuple localPoint);

        internal Tuple WorldToObject(Tuple point)
        {
            if (Parent != null)
                point = Parent.WorldToObject(point);

            return Transform.Inverse.Transform(point);
        }

        internal Tuple NormalToWorld(Tuple normal)
        {
            normal = Transform.Inverse.Transpose.Transform(normal);
            normal.w = 0;
            normal = normal.Normalize;

            if (Parent != null)
                normal = Parent.NormalToWorld(normal);

            return normal;
        }
    }
}
