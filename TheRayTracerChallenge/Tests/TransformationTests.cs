using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class TransformationTests
    {
        [Test]
        public void Multiplying_by_a_translation_matrix()
        {
            var transformation = Transformation.Translation(5, -3, 2);
            var point = Tuple.Point(-3, 4, 5);

            var translatedPoint = transformation.Transform(point);

            Assert.AreEqual(Tuple.Point(2, 1, 7), translatedPoint);
        }

        [Test]
        public void Multiplying_by_the_inverse_of_a_translation_matrix()
        {
            var transformation = Transformation.Translation(5, -3, 2);
            var inv = transformation.Inverse;
            var point = Tuple.Point(-3, 4, 5);

            var translatedPoint = inv.Transform(point);

            Assert.AreEqual(Tuple.Point(-8, 7, 3), translatedPoint);
        }

        [Test]
        public void Translation_does_not_affect_vectors()
        {
            var transformation = Transformation.Translation(5, -3, 2);
            var v = Tuple.Vector(-3, 4, 5);

            var translatedVector = transformation.Transform(v);

            Assert.AreEqual(v, translatedVector);
        }

        [Test]
        public void A_scaling_matrix_applied_to_a_point()
        {
            var transformation = Transformation.Scaling(2, 3, 4);
            var p = Tuple.Point(-4, 6, 8);

            var scaledPoint = transformation.Transform(p);

            Assert.AreEqual(Tuple.Point(-8, 18, 32), scaledPoint);
        }

        [Test]
        public void A_scaling_matrix_applied_to_a_vector()
        {
            var transformation = Transformation.Scaling(2, 3, 4);
            var v = Tuple.Vector(-4, 6, 8);

            var scaledVector = transformation.Transform(v);

            Assert.AreEqual(Tuple.Vector(-8, 18, 32), scaledVector);
        }

        [Test]
        public void Multiplying_by_the_inverse_of_a_scaling_matrix()
        {
            var transformation = Transformation.Scaling(2, 3, 4);
            var inv = transformation.Inverse;
            var v = Tuple.Vector(-4, 6, 8);

            var scaledVector = inv.Transform(v);

            Assert.AreEqual(Tuple.Vector(-2, 2, 2), scaledVector);
        }

        [Test]
        public void Reflection_is_scaling_by_a_negative_value()
        {
            var transformation = Transformation.Scaling(-1, 1, 1);
            var p = Tuple.Point(2, 3, 4);

            var reflectedPoint = transformation.Transform(p);

            Assert.AreEqual(Tuple.Point(-2, 3, 4), reflectedPoint);
        }

        [Test]
        public void Rotating_a_point_around_the_x_axis()
        {
            var p = Tuple.Point(0, 1, 0);
            var halfQuarter = Transformation.RotationX(Math.PI / 4);
            var fullQuarter = Transformation.RotationX(Math.PI / 2);

            var pointRotatedHalfQuarter = halfQuarter.Transform(p);
            var pointRotatedFullQuarter = fullQuarter.Transform(p);

            Assert.AreEqual(Tuple.Point(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2), pointRotatedHalfQuarter);
            Assert.AreEqual(Tuple.Point(0, 0, 1), pointRotatedFullQuarter);
        }

        [Test]
        public void The_inverse_of_an_x_rotation_rotates_in_the_opposite_direction()
        {
            var p = Tuple.Point(0, 1, 0);
            var halfQuarter = Transformation.RotationX(Math.PI / 4);
            var inv = halfQuarter.Inverse;

            var rotatedPoint = inv.Transform(p);

            Assert.AreEqual(Tuple.Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2), rotatedPoint);
        }

        [Test]
        public void Rotating_a_point_around_the_y_axis()
        {
            var p = Tuple.Point(0, 0, 1);
            var halfQuarter = Transformation.RotationY(Math.PI / 4);
            var fullQuarter = Transformation.RotationY(Math.PI / 2);

            var pointRotatedHalfQuarter = halfQuarter.Transform(p);
            var pointRotatedFullQuarter = fullQuarter.Transform(p);

            Assert.AreEqual(Tuple.Point(Math.Sqrt(2) / 2, 0, Math.Sqrt(2) / 2), pointRotatedHalfQuarter);
            Assert.AreEqual(Tuple.Point(1, 0, 0), pointRotatedFullQuarter);
        }

        [Test]
        public void Rotating_a_point_around_the_z_axis()
        {
            var p = Tuple.Point(0, 1, 0);
            var halfQuarter = Transformation.RotationZ(Math.PI / 4);
            var fullQuarter = Transformation.RotationZ(Math.PI / 2);

            var pointRotatedHalfQuarter = halfQuarter.Transform(p);
            var pointRotatedFullQuarter = fullQuarter.Transform(p);

            Assert.AreEqual(Tuple.Point(-Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0), pointRotatedHalfQuarter);
            Assert.AreEqual(Tuple.Point(-1, 0, 0), pointRotatedFullQuarter);
        }

        [Test]
        public void A_shearing_transformation_moves_x_in_proprtion_to_y()
        {
            var transformation = Transformation.Shearing(1, 0, 0, 0, 0, 0);
            var p = Tuple.Point(2, 3, 4);

            var shearedPoint = transformation.Transform(p);

            Assert.AreEqual(Tuple.Point(5, 3, 4), shearedPoint);
        }

        [Test]
        public void A_shearing_transformation_moves_x_in_proprtion_to_z()
        {
            var transformation = Transformation.Shearing(0, 1, 0, 0, 0, 0);
            var p = Tuple.Point(2, 3, 4);

            var shearedPoint = transformation.Transform(p);

            Assert.AreEqual(Tuple.Point(6, 3, 4), shearedPoint);
        }

        [Test]
        public void A_shearing_transformation_moves_y_in_proprtion_to_x()
        {
            var transformation = Transformation.Shearing(0, 0, 1, 0, 0, 0);
            var p = Tuple.Point(2, 3, 4);

            var shearedPoint = transformation.Transform(p);

            Assert.AreEqual(Tuple.Point(2, 5, 4), shearedPoint);
        }

        [Test]
        public void A_shearing_transformation_moves_y_in_proprtion_to_z()
        {
            var transformation = Transformation.Shearing(0, 0, 0, 1, 0, 0);
            var p = Tuple.Point(2, 3, 4);

            var shearedPoint = transformation.Transform(p);

            Assert.AreEqual(Tuple.Point(2, 7, 4), shearedPoint);
        }

        [Test]
        public void A_shearing_transformation_moves_z_in_proprtion_to_x()
        {
            var transformation = Transformation.Shearing(0, 0, 0, 0, 1, 0);
            var p = Tuple.Point(2, 3, 4);

            var shearedPoint = transformation.Transform(p);

            Assert.AreEqual(Tuple.Point(2, 3, 6), shearedPoint);
        }

        [Test]
        public void A_shearing_transformation_moves_z_in_proprtion_to_y()
        {
            var transformation = Transformation.Shearing(0, 0, 0, 0, 0, 1);
            var p = Tuple.Point(2, 3, 4);

            var shearedPoint = transformation.Transform(p);

            Assert.AreEqual(Tuple.Point(2, 3, 7), shearedPoint);
        }

        [Test]
        public void The_transformation_matrix_for_the_default_orientation()
        {
            var from = Tuple.Point(0, 0, 0);
            var to = Tuple.Point(0, 0, -1);
            var up = Tuple.Vector(0, 1, 0);

            var transform = Transformation.ViewTransform(from, to, up);

            Assert.AreEqual(Transformation.Identity, transform);
        }

        [Test]
        public void A_view_transformation_matrix_looking_in_positive_z_direction()
        {
            var from = Tuple.Point(0, 0, 0);
            var to = Tuple.Point(0, 0, 1);
            var up = Tuple.Vector(0, 1, 0);

            var transform = Transformation.ViewTransform(from, to, up);

            Assert.AreEqual(Transformation.Scaling(-1, 1, -1), transform);
        }

        [Test]
        public void The_view_transformation_moves_the_world()
        {
            var from = Tuple.Point(0, 0, 8);
            var to = Tuple.Point(0, 0, 0);
            var up = Tuple.Vector(0, 1, 0);

            var transform = Transformation.ViewTransform(from, to, up);

            Assert.AreEqual(Transformation.Translation(0, 0, -8), transform);
        }

        [Test]
        public void An_arbitrary_view_transformation()
        {
            var from = Tuple.Point(1, 3, 2);
            var to = Tuple.Point(4, -2, 8);
            var up = Tuple.Vector(1, 1, 0);

            var transform = Transformation.ViewTransform(from, to, up);

            Matrix<double> expected = DenseMatrix.OfArray(new double[,] {
                {-0.50709,  0.50709,  0.67612, -2.36643 },
                { 0.76772,  0.60609,  0.12122, -2.82843 },
                { -0.35857, 0.59761, -0.71714,  0.00000 },
                { 0.00000 , 0.00000,  0.00000,  1.00000 } });

            // The expected matrix values from the books are rounded, 
            Assert.AreEqual(expected.ToString("F5"), transform.Matrix.ToString("F5"));
        }
    }
}
