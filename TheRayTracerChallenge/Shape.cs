using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
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

        public IntersectionCollection Intersect(Ray ray)
        {
            var localRay = ray.Transform(Transform.Inverse);
            return LocalIntersect(localRay);
        }

        protected abstract IntersectionCollection LocalIntersect(Ray localRay);
    }
}
