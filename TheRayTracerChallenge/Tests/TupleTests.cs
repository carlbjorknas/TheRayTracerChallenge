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
        }
    }
}
