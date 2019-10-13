﻿using System;
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

        public Tuple NormalAt(Tuple point)
        {
            var localPoint = Transform.Inverse.Transform(point);
            var localNormal = LocalNormalAt(localPoint);
            var worldNormal = Transform.Inverse.Transpose.Transform(localNormal);
            worldNormal.w = 0;
            return worldNormal.Normalize;
        }

        protected abstract Tuple LocalNormalAt(Tuple localPoint);
    }
}
