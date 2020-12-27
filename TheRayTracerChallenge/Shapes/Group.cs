using System;
using System.Collections.Generic;
using System.Linq;
using TheRayTracerChallenge.Shapes.Utils;
using TheRayTracerChallenge.Utils;

namespace TheRayTracerChallenge.Shapes
{
    class Group : Shape
    {
        private Bounds _bounds;

        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            if (!Shapes.Any() || !HitsBoundingBox(localRay))
                return new IntersectionCollection();

            var xs = Shapes
                .SelectMany(shape => shape.Intersect(localRay).Intersections)
                .OrderBy(intersection => intersection.T);
            return new IntersectionCollection(xs);
        }

        private bool HitsBoundingBox(Ray localRay)
        {
            var bounds = Bounds;
            var (xtMin, xtMax) = CheckAxis(localRay.Origin.x, localRay.Direction.x, bounds.Min.x, bounds.Max.x);
            var (ytMin, ytMax) = CheckAxis(localRay.Origin.y, localRay.Direction.y, bounds.Min.y, bounds.Max.y);
            var (ztMin, ztMax) = CheckAxis(localRay.Origin.z, localRay.Direction.z, bounds.Min.z, bounds.Max.z);

            var tMin = new[] { xtMin, ytMin, ztMin }.Max();
            var tMax = new[] { xtMax, ytMax, ztMax }.Min();

            if (tMin > tMax)
                return false;

            return true;
        }

        private (double tMin, double tMax) CheckAxis(double origin, double direction, double min, double max)
        {
            var tMinNumerator = min - origin;
            var tMaxNumerator = max - origin;

            double tMin, tMax;
            if (Math.Abs(direction) >= C.Epsilon)
            {
                tMin = tMinNumerator / direction;
                tMax = tMaxNumerator / direction;
            }
            else
            {
                tMin = tMinNumerator * double.PositiveInfinity;
                tMax = tMaxNumerator * double.PositiveInfinity;
            }

            if (tMin > tMax)
                Swapper.Swap(ref tMin, ref tMax);

            return (tMin, tMax);
        }

        public override Tuple LocalNormalAt(Tuple localPoint)
        {
            throw new NotImplementedException();
        }

        public List<Shape> Shapes { get; } = new List<Shape>();

        public override Bounds Bounds
        {
            get
            {
                if (_bounds == null)
                {
                    var groupSpaceBounds = Shapes
                        .Select(s => s.Bounds.Transform(s.Transform))
                        .ToList();

                    var minX = groupSpaceBounds.Min(b => b.Min.x);
                    var minY = groupSpaceBounds.Min(b => b.Min.y);
                    var minZ = groupSpaceBounds.Min(b => b.Min.z);
                    var maxX = groupSpaceBounds.Max(b => b.Max.x);
                    var maxY = groupSpaceBounds.Max(b => b.Max.y);
                    var maxZ = groupSpaceBounds.Max(b => b.Max.z);

                    _bounds = new Bounds(Tuple.Point(minX, minY, minZ), Tuple.Point(maxX, maxY, maxZ));
                }

                return _bounds;
            }
        }

        internal void AddChild(Shape shape)
        {
            Shapes.Add(shape);
            shape.Parent = this;
        }
    }
}
