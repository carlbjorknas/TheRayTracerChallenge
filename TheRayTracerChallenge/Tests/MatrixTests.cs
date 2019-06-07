using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void A_2_X_2_matrix_ought_to_be_representable()
        {
            Matrix<double> M = DenseMatrix.OfArray(new double[,] {
                {-3, 5 },
                {1, -2 }});

            Assert.AreEqual(-3, M[0, 0]);
            Assert.AreEqual(5, M[0, 1]);
            Assert.AreEqual(1, M[1, 0]);
            Assert.AreEqual(-2, M[1, 1]);
        }

        [Test]
        public void A_3_X_3_matrix_ought_to_be_representable()
        {
            Matrix<double> M = DenseMatrix.OfArray(new double[,] {
                {-3, 5, 0},
                {1, -2, -7},
                {0, 1, 1} });

            Assert.AreEqual(-3, M[0, 0]);
            Assert.AreEqual(-2, M[1, 1]);
            Assert.AreEqual(1, M[2, 2]);
        }

        [Test]
        public void Matrix_equality_with_identical_matrices()
        {
            Matrix<double> M1 = DenseMatrix.OfArray(new double[,] {
                {1, 2, 3, 4},
                {5,6,7,8},
                {9,8,7,6},
                {5,4,3,2 } });

            Matrix<double> M2 = DenseMatrix.OfArray(new double[,] {
                {1, 2, 3, 4},
                {5,6,7,8},
                {9,8,7,6},
                {5,4,3,2 } });

            Assert.AreEqual(M1, M2);
        }

        [Test]
        public void Matrix_equality_with_different_matrices()
        {
            Matrix<double> M1 = DenseMatrix.OfArray(new double[,] {
                {1, 2, 3, 4},
                {5,6,7,8},
                {9,8,7,6},
                {5,4,3,2 } });

            Matrix<double> M2 = DenseMatrix.OfArray(new double[,] {
                {2, 3, 4, 5},
                {6,7,8, 9},
                {8,7,6,5},
                {4,3,2,1 } });

            Assert.AreNotEqual(M1, M2);
        }

        [Test]
        public void Multiplying_two_matrices()
        {
            Matrix<double> M1 = DenseMatrix.OfArray(new double[,] {
                {1, 2, 3, 4},
                {5,6,7,8},
                {9,8,7,6},
                {5,4,3,2 } });

            Matrix<double> M2 = DenseMatrix.OfArray(new double[,] {
                {-2, 1, 2, 3},
                {3,2,1, -1},
                {4,3,6,5},
                {1,2,7,8 } });

            var product = M1 * M2;

            Matrix<double> expected = DenseMatrix.OfArray(new double[,] {
                {20, 22, 50, 48},
                {44,54,114, 108},
                {40,58,110,102},
                {16,26,46,42 } });

            Assert.AreEqual(expected, product);
        }

        [Test]
        public void A_matrix_multiplied_by_a_tuple()
        {
            Matrix<double> M = DenseMatrix.OfArray(new double[,] {
                {1, 2, 3, 4},
                {2,4,4,2},
                {8,6,4,1},
                {0,0,0,1} });
            var V = Vector<double>.Build.Dense(new double[] { 1, 2, 3, 1 });
            var result = M * V;

            var expected = Vector<double>.Build.Dense(new double[] { 18, 24, 33, 1 });

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Transposing_a_matrix()
        {
            Matrix<double> M = DenseMatrix.OfArray(new double[,] {
                {0,9,3,0},
                {9,8,0,8},
                {1,8,5,3},
                {0,0,5,8} });

            Matrix<double> expected = DenseMatrix.OfArray(new double[,] {
                {0,9,1,0},
                {9,8,8,0},
                {3,0,5,5},
                {0,8,3,8} });

            Assert.AreEqual(expected, M.Transpose());
        }

        [Test]
        public void Calculating_the_inverse_of_a_matrix()
        {
            Matrix<double> M = DenseMatrix.OfArray(new double[,] {
                {-5,2,6,-8},
                {1,-5,1,8},
                {7,7,-6,-7},
                {1,-3,7,4} });

            Matrix<double> expected = DenseMatrix.OfArray(new double[,] {
                {0.21805,0.45113,0.24060,-0.04511},
                {-0.80827,-1.45677,-0.44361,0.52068},
                {-0.07895,-0.22368,-0.05263,0.19737},
                {-0.52256,-0.81391,-0.30075,0.30639} });

            // The expected matrix values from the books are rounded, 
            Assert.AreEqual(expected.ToString("F5"), M.Inverse().ToString("F5"));
        }
    }
}
