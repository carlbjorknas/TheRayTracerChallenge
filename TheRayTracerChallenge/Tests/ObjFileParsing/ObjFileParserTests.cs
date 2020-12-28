using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.ObjFileParsing;

namespace TheRayTracerChallenge.Tests.ObjFileParsing
{
    [TestFixture]
    public class ObjFileParserTests
    {
        [Test]
        public void Ignoring_unrecognized_lines()
        {
            var gibberish =
@"There was a young lady named Bright
who traveled much faster than light.
She set out one day
in a relative way,
and came back the previous night.";
            var result = ObjFileParser.Parse(gibberish);
            result.NumberIgnoredLines.Should().Be(5);
        }
    }
}
