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
    }
}
