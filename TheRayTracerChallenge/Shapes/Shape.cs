using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes.Utils;

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

        public Shape Parent { get; set; }

        public IntersectionCollection Intersect(Ray ray)
        {
            var localRay = ray.Transform(Transform.Inverse);
            return LocalIntersect(localRay);
        }

        public abstract Bounds Bounds { get; }

        public abstract IntersectionCollection LocalIntersect(Ray localRay);

        public Tuple NormalAt(Tuple point, Intersection? i = null)
        {
            var localPoint = WorldToObject(point);
            var localNormal = LocalNormalAt(localPoint, i);
            return NormalToWorld(localNormal);
        }

        public abstract Tuple LocalNormalAt(Tuple localPoint, Intersection? i = null);

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

        public virtual bool Contains(Shape other) => this == other;
    }
}
