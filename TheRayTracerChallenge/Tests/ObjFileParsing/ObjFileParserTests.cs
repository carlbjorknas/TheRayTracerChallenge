using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.ObjFileParsing;
using TheRayTracerChallenge.Shapes;

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

        [Test]
        public void Vertex_records()
        {
            var content =
@"v -1 1 0
v -1.0000 0.5000 0.0000
v 1 0 0
v 1 1 0";
            var result = ObjFileParser.Parse(content);
            result.Vertices.Count.Should().Be(5); // One dummy a index 0
            result.Vertices[1].Should().Be(Tuple.Point(-1, 1, 0));
            result.Vertices[2].Should().Be(Tuple.Point(-1, 0.5, 0));
            result.Vertices[3].Should().Be(Tuple.Point(1, 0, 0));
            result.Vertices[4].Should().Be(Tuple.Point(1, 1, 0));
        }

        [Test]
        public void Parsing_triangle_faces()
        {
            var content =
@"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0

f 1 2 3
f 1 3 4";
            var result = ObjFileParser.Parse(content);
            
            var triangle1 = (Triangle)result.DefaultGroup.Shapes[0];
            triangle1.P1.Should().Be(result.Vertices[1]);
            triangle1.P2.Should().Be(result.Vertices[2]);
            triangle1.P3.Should().Be(result.Vertices[3]);
            
            var triangle2 = (Triangle)result.DefaultGroup.Shapes[1];
            triangle2.P1.Should().Be(result.Vertices[1]);
            triangle2.P2.Should().Be(result.Vertices[3]);
            triangle2.P3.Should().Be(result.Vertices[4]);
        }
    }
}
