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
        public Transformation Transform { get; }
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
    }
}
