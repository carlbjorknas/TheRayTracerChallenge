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
    }
}
