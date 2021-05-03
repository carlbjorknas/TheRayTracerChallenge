using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRayTracerChallenge.Shapes.Utils
{
    class Bounds
    {
        public Bounds(Tuple min, Tuple max)
        {
            Min = min;
            Max = max;
        }

        public Tuple Min { get; }
        public Tuple Max { get; }

        public Bounds Transform(Transformation transformation)
        {
            var corners = new List<Tuple>
            {
                Min,
                Tuple.Point(Min.x, Min.y, Max.z), // Left, bottom, inward
                Tuple.Point(Min.x, Max.y, Max.z), // Left, top, inward
                Tuple.Point(Min.x, Max.y, Min.z), // Left, top, outward
                Max,
                Tuple.Point(Max.x, Min.y, Max.z), // Right, bottom, inward
                Tuple.Point(Max.x, Min.y, Min.z), // Right, bottom, outward
                Tuple.Point(Max.x, Max.y, Min.z), // Right, top, outward
            };

            var transformedCorners = corners
                .Select(corner => transformation.Transform(corner))
                .ToList();
            var maxX = transformedCorners.Max(tc => tc.x);
            var maxY = transformedCorners.Max(tc => tc.y);
            var maxZ = transformedCorners.Max(tc => tc.z);
            var minX = transformedCorners.Min(tc => tc.x);
            var minY = transformedCorners.Min(tc => tc.y);
            var minZ = transformedCorners.Min(tc => tc.z);

            return new Bounds(Tuple.Point(minX, minY, minZ), Tuple.Point(maxX, maxY, maxZ));
        }

        public override string ToString()
        {
            return $"Min: {Min.x}, {Min.y}, {Min.z} Max: {Max.x}, {Max.y}, {Max.z}";
        }
    }
}
