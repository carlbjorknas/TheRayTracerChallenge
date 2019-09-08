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
        }

        public double HSize { get; }
        public double VSize { get; }
        public double FieldOfView { get; }
        public Transformation Transform { get; }
    }
}
