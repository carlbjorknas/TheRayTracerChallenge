using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    class Transformation
    {
        Matrix<double> _matrix;

        private Transformation(Matrix<double> matrix)
        {
            _matrix = matrix;
        }

        public Transformation Inverse
            => new Transformation(_matrix.Inverse());

        public static Transformation Translation(double x, double y, double z)
        {
            var matrix = Matrix<double>.Build.DenseIdentity(4, 4);
            matrix.SetColumn(3, new double[] { x, y, z, 1 });
            return new Transformation(matrix);
        }

        internal static Transformation Scaling(double x, double y, double z)
        {
            var matrix = Matrix<double>.Build.Dense(4, 4);
            matrix.SetDiagonal(new[] { x, y, z, 1 });            
            return new Transformation(matrix);
        }

        internal static Transformation RotationX(double radians)
        {
            var matrix = Matrix<double>.Build.DenseIdentity(4, 4);
            matrix[1, 1] = Math.Cos(radians);
            matrix[1, 2] = -Math.Sin(radians);
            matrix[2, 1] = Math.Sin(radians);
            matrix[2, 2] = Math.Cos(radians);

            return new Transformation(matrix);
        }

        internal static Transformation RotationY(double radians)
        {
            var matrix = Matrix<double>.Build.DenseIdentity(4, 4);
            matrix[0, 0] = Math.Cos(radians);
            matrix[0, 2] = Math.Sin(radians);
            matrix[2, 0] = -Math.Sin(radians);
            matrix[2, 2] = Math.Cos(radians);

            return new Transformation(matrix);
        }

        internal static Transformation RotationZ(double radians)
        {
            var matrix = Matrix<double>.Build.DenseIdentity(4, 4);
            matrix[0, 0] = Math.Cos(radians);
            matrix[0, 1] = -Math.Sin(radians);
            matrix[1, 0] = Math.Sin(radians);
            matrix[1, 1] = Math.Cos(radians);

            return new Transformation(matrix);
        }

        internal static Transformation Shearing(double xy, double xz, double yx, double yz, double zx, double zy)
        {
            var matrix = Matrix<double>.Build.DenseIdentity(4, 4);
            matrix[0, 1] = xy;
            matrix[0, 2] = xz;
            matrix[1, 0] = yx;
            matrix[1, 2] = yz;
            matrix[2, 0] = zx;
            matrix[2, 1] = zy;

            return new Transformation(matrix);
        }

        public Tuple Transform(Tuple tuple)
        {
            var vector = Vector<double>.Build.Dense(new[] { tuple.x, tuple.y, tuple.z, tuple.w });
            var newVec = _matrix * vector;
            return new Tuple(newVec[0], newVec[1], newVec[2], newVec[3]);
        }
    }
}
