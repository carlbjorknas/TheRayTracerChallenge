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
            Assert.AreEqual(t, new Tuple(4, -4, 3, 0));
        }
    }
}
