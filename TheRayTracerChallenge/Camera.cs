using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    class Camera
    {
        public Camera(int hSize, int vSize, double fieldOfView)
        {
            HSize = hSize;
            VSize = vSize;
            FieldOfView = fieldOfView;
            Transform = Transformation.Identity;
            PrecalculateAndSetCanvasData();
        }

        private double _halfWidth;
        private double _halfHeight;

        public double HSize { get; }
        public double VSize { get; }
        public double FieldOfView { get; }
        public Transformation Transform { get; set; }
        public double PixelSize { get; internal set; }        

        private void PrecalculateAndSetCanvasData()
        {
            var halfView = Math.Tan(FieldOfView / 2);
            var aspect = HSize / VSize;

            if (aspect >= 1)
            {
                _halfWidth = halfView;
                _halfHeight = halfView / aspect;
            }
            else
            {
                _halfWidth = halfView * aspect;
                _halfHeight = halfView;
            }

            PixelSize = (_halfWidth * 2) / HSize;
        }

        internal Ray RayForPixel(int x, int y)
        {
            // The offset from the edge of the canvas to the pixel's center
            var xOffset = (x + 0.5) * PixelSize;
            var yOffset = (y + 0.5) * PixelSize;

            // the untransformed coordinates of the pixel in world space.
            // (remember that the camera looks toward -z, so +x is to the *left*.)
            var worldX = _halfWidth - xOffset;
            var worldY = _halfHeight - yOffset;

            // using the camera matrix, transform the canvas point and the origin,
            // and then compute the ray's direction vector.
            // (remember that the canvas is at z=-1)
            var pixel = Transform.Inverse.Transform(Tuple.Point(worldX, worldY, -1));
            var origin = Transform.Inverse.Transform(Tuple.Point(0, 0, 0));
            var direction = (pixel - origin).Normalize;

            return new Ray(origin, direction);
        }
    }
}
