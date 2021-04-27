using TheRayTracerChallenge.Shapes;
using TheRayTracerChallenge.Shapes.Utils;

namespace TheRayTracerChallenge.Tests.Shapes
{
    class TestShape : Shape
    {
        public Ray LocalRay { get; private set; }

        public override Bounds Bounds => throw new System.NotImplementedException();

        public override IntersectionCollection LocalIntersect(Ray localRay)
        {
            LocalRay = localRay;
            return new IntersectionCollection();
        }

        public override Tuple LocalNormalAt(Tuple localPoint, Intersection? i = null)
            => Tuple.Vector(localPoint.x, localPoint.y, localPoint.z);
    }
}
