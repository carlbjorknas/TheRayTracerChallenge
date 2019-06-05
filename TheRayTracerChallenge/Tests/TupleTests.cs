using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.Tests
{
    [TestFixture]
    public class TupleTests
    {
        [Test]
        public void A_tuple_with_w_equal_to_1_is_a_point()
        {
            var t = new Tuple(4.3, -4.2, 3.1, 1.0);
            Assert.IsTrue(t.IsPoint);
            Assert.IsFalse(t.IsVector);
        }

        [Test]
        public void A_tuple_with_w_equal_to_0_is_a_vector()
        {
            var t = new Tuple(4.3, -4.2, 3.1, 0.0);
            Assert.IsFalse(t.IsPoint);
            Assert.IsTrue(t.IsVector);
        }

        [Test]
        public void Point_creates_tuples_with_w_equal_to_1()
        {
            var t = Tuple.Point(4, -4, 3);
            Assert.AreEqual(t, new Tuple(4, -4, 3, 1));
        }

        [Test]
        public void Vector_creates_tuples_with_w_equal_to_0()
        {
            var t = Tuple.Vector(4, -4, 3);
            Assert.AreEqual(new Tuple(4, -4, 3, 0), t);
        }

        [Test]
        public void Adding_two_tuples()
        {
            var t1 = new Tuple(3, -2, 5, 1);
            var t2 = new Tuple(-2, 3, 1, 0);
            var sum = t1 + t2;
            Assert.AreEqual(new Tuple(1, 1, 6, 1), sum);
        }

        [Test]
        public void Subtracting_two_points()
        {
            var p1 = Tuple.Point(3, 2, 1);
            var p2 = Tuple.Point(5, 6, 7);
            var diff = p1 - p2;
            Assert.AreEqual(Tuple.Vector(-2, -4, -6), diff);
        }

        [Test]
        public void Subtracting_a_vector_from_a_point()
        {
            var p = Tuple.Point(3, 2, 1);
            var v = Tuple.Vector(5, 6, 7);
            var diff = p - v;
            Assert.AreEqual(Tuple.Point(-2, -4, -6), diff);
        }

        [Test]
        public void Subtraction_two_vectors()
        {
            var v1 = Tuple.Vector(3, 2, 1);
            var v2 = Tuple.Vector(5, 6, 7);
            var diff = v1 - v2;
            Assert.AreEqual(Tuple.Vector(-2, -4, -6), diff);
        }

        [Test]
        public void Subracting_a_vector_from_the_zero_vector()
        {
            var zero = Tuple.Vector(0, 0, 0);
            var v = Tuple.Vector(1, -2, 3);
            var diff = zero - v;
            Assert.AreEqual(Tuple.Vector(-1, 2, -3), diff);
        }

        [Test]
        public void Negating_a_tuple()
        {
            var t = new Tuple(1, -2, 3, -4);
            var negated = -t;
            Assert.AreEqual(new Tuple(-1, 2, -3, 4), negated);
        }

        [Test]
        public void Multiplying_a_tuple_with_a_scalar()
        {
            var t = new Tuple(1, -2, 3, -4);
            var scaled = t * 3.5;
            Assert.AreEqual(new Tuple(3.5, -7, 10.5, -14), scaled);
        }

        [Test]
        public void Multiplying_a_tuple_with_a_fraction()
        {
            var t = new Tuple(1, -2, 3, -4);
            var scaled = t * 0.5;
            Assert.AreEqual(new Tuple(0.5, -1, 1.5, -2), scaled);
        }

        [Test]
        public void Dividing_a_tuple_with_a_scalar()
        {
            var t = new Tuple(1, -2, 3, -4);
            var scaled = t / 2;
            Assert.AreEqual(new Tuple(0.5, -1, 1.5, -2), scaled);
        }

        [Test]
        public void Computing_the_magnitude_of_vector_1_0_0()
        {
            var v = Tuple.Vector(1, 0, 0);
            Assert.AreEqual(1, v.Magnitude);
        }

        [Test]
        public void Computing_the_magnitude_of_vector_0_1_0()
        {
            var v = Tuple.Vector(0, 1, 0);
            Assert.AreEqual(1, v.Magnitude);
        }

        [Test]
        public void Computing_the_magnitude_of_vector_0_0_1()
        {
            var v = Tuple.Vector(0, 0, 1);
            Assert.AreEqual(1, v.Magnitude);
        }

        [Test]
        public void Computing_the_magnitude_of_vector_1_2_3()
        {
            var v = Tuple.Vector(1, 2, 3);
            Assert.AreEqual(Math.Sqrt(14), v.Magnitude);
        }

        [Test]
        public void Computing_the_magnitude_of_vector_neg1_neg2_neg3()
        {
            var v = Tuple.Vector(-1, -2, -3);
            Assert.AreEqual(Math.Sqrt(14), v.Magnitude);
        }

        [Test]
        public void Normalizing_vector_4_0_0_gives_1_0_0()
        {
            var v = Tuple.Vector(4, 0, 0);
            var normalized = v.Normalize;
            Assert.AreEqual(Tuple.Vector(1, 0, 0), normalized);
        }

        [Test]
        public void Normalizing_vector_1_2_3()
        {
            var v = Tuple.Vector(1, 2, 3);
            var normalized = v.Normalize;
            Assert.AreEqual(Tuple.Vector(0.26726, 0.53452, 0.80178), normalized);
        }

        [Test]
        public void Magnitude_of_a_normalized_vector()
        {
            var v = Tuple.Vector(1, 2, 3);
            var normalized = v.Normalize;
            Assert.AreEqual(1, normalized.Magnitude);
        }

        [Test]
        public void The_dot_product_of_two_vectors()
        {
            var v1 = Tuple.Vector(1, 2, 3);
            var v2 = Tuple.Vector(2, 3, 4);
            var dotProduct = v1.Dot(v2);
            Assert.AreEqual(20, dotProduct);
        }

        [Test]
        public void The_cross_product_of_two_vectors()
        {
            var v1 = Tuple.Vector(1, 2, 3);
            var v2 = Tuple.Vector(2, 3, 4);

            var cross1 = v1.Cross(v2);
            Assert.AreEqual(Tuple.Vector(-1, 2, -1), cross1);

            var cross2 = v2.Cross(v1);
            Assert.AreEqual(Tuple.Vector(1, -2, 1), cross2);
        }
    }
}
