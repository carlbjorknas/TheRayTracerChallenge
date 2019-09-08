using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    class Transformation
    {
        private Transformation(Matrix<double> matrix)
        {
            Matrix = matrix;
        }

        public Transformation Inverse
            => new Transformation(Matrix.Inverse());

        public Transformation Transpose
            => new Transformation(Matrix.Transpose());

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

        internal static Transformation ViewTransform(Tuple from, Tuple to, Tuple up)
        {
            var forwardVec = (to - from).Normalize;
            var leftVec = forwardVec.Cross(up.Normalize);
            var trueUpVec = leftVec.Cross(forwardVec);

            var orientationMatrix = Matrix<double>.Build.DenseIdentity(4, 4);
            orientationMatrix[0, 0] = leftVec.x;
            orientationMatrix[0, 1] = leftVec.y;
            orientationMatrix[0, 2] = leftVec.z;
            orientationMatrix[1, 0] = trueUpVec.x;
            orientationMatrix[1, 1] = trueUpVec.y;
            orientationMatrix[1, 2] = trueUpVec.z;
            orientationMatrix[2, 0] = -forwardVec.x;
            orientationMatrix[2, 1] = -forwardVec.y;
            orientationMatrix[2, 2] = -forwardVec.z;
            orientationMatrix[3, 3] = 1;

            var translation = Translation(-from.x, -from.y, -from.z);

            return new Transformation(orientationMatrix).Chain(translation);
        }

        internal static Transformation Identity { get; } = new Transformation(Matrix<double>.Build.DenseIdentity(4, 4));        

        public Tuple Transform(Tuple tuple)
        {
            var vector = Vector<double>.Build.Dense(new[] { tuple.x, tuple.y, tuple.z, tuple.w });
            var newVec = Matrix * vector;
            return new Tuple(newVec[0], newVec[1], newVec[2], newVec[3]);
        }

        public Transformation Chain(Transformation transformation)
        {
            return new Transformation(Matrix * transformation.Matrix);
        }

        public Matrix<double> Matrix { get; }

        public override string ToString()
        {
            return $"{base.ToString()} {Matrix}";
        }

        public override bool Equals(object obj)
            => obj is Transformation t && Matrix.Equals(t.Matrix);

        public override int GetHashCode()
            => Matrix.GetHashCode();
    }
}
