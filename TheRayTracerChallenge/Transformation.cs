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

        public static Transformation Translation(double x, double y, double z)
        {
            var matrix = Matrix<double>.Build.DenseIdentity(4, 4);
            matrix.SetColumn(3, new double[] { x, y, z, 1 });
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
