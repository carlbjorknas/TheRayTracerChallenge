﻿using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheRayTracerChallenge.ObjFileParsing;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.Tests.ObjFileParsing
{
    [TestFixture]
    public class ObjFileParserTests
    {
        public string _objFilesDir { get; private set; }

        [SetUp]
        public void Init()
        {
            _objFilesDir = Path.Combine(Directory.GetCurrentDirectory(), "Tests", "ObjFileParsing");
        }

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

        [Test]
        public void Triangulating_polygons()
        {
            var content =
@"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0
v 0 2 0

f 1 2 3 4 5";
            var result = ObjFileParser.Parse(content);

            result.DefaultGroup.Shapes.Count.Should().Be(3);

            var triangle1 = (Triangle)result.DefaultGroup.Shapes[0];
            triangle1.P1.Should().Be(result.Vertices[1]);
            triangle1.P2.Should().Be(result.Vertices[2]);
            triangle1.P3.Should().Be(result.Vertices[3]);

            var triangle2 = (Triangle)result.DefaultGroup.Shapes[1];
            triangle2.P1.Should().Be(result.Vertices[1]);
            triangle2.P2.Should().Be(result.Vertices[3]);
            triangle2.P3.Should().Be(result.Vertices[4]);

            var triangle3 = (Triangle)result.DefaultGroup.Shapes[2];
            triangle3.P1.Should().Be(result.Vertices[1]);
            triangle3.P2.Should().Be(result.Vertices[4]);
            triangle3.P3.Should().Be(result.Vertices[5]);
        }

        [Test]
        public void Triangles_in_named_groups()
        {
            var path =  Path.Combine(_objFilesDir, "triangles.obj");
            var parser = ObjFileParser.ParseFromFile(path);

            var group1 = parser.GetGroup("FirstGroup");
            var triangle1 = (Triangle)group1.Shapes[0];
            triangle1.P1.Should().Be(parser.Vertices[1]);
            triangle1.P2.Should().Be(parser.Vertices[2]);
            triangle1.P3.Should().Be(parser.Vertices[3]);

            var group2 = parser.GetGroup("SecondGroup");
            var triangle2 = (Triangle)group2.Shapes[0];
            triangle2.P1.Should().Be(parser.Vertices[1]);
            triangle2.P2.Should().Be(parser.Vertices[3]);
            triangle2.P3.Should().Be(parser.Vertices[4]);
        }

        [Test]
        public void Converting_an_OBJ_file_to_a_group()
        {
            var path = Path.Combine(_objFilesDir, "triangles.obj");
            var parser = ObjFileParser.ParseFromFile(path);

            var bundlingGroup = parser.BundlingGroup;
            bundlingGroup.Shapes.Should().Contain(parser.GetGroup("FirstGroup"));
            bundlingGroup.Shapes.Should().Contain(parser.GetGroup("SecondGroup"));
        }

        [Test]
        public void Vertex_normal_records()
        {
            var content =
@"vn 0 0 1
vn 0.707 0 -0.707
vn 1 2 3";
            var parser = ObjFileParser.Parse(content);

            parser.Normals[1].Should().Be(Tuple.Vector(0, 0, 1));
            parser.Normals[2].Should().Be(Tuple.Vector(0.707, 0, -0.707));
            parser.Normals[3].Should().Be(Tuple.Vector(1, 2, 3));
        }

        [Test]
        public void FacesWithNormals()
        {
            var content =
@"v 0 1 0
v -1 0 0
v 1 0 0

vn -1 0 0
vn 1 0 0
vn 0 1 0

f 1//3 2//1 3//2
f 1/0/3 2/102/1 3/14/2";

            var parser = ObjFileParser.Parse(content);
            var g = parser.DefaultGroup;
            var t1 = (SmoothTriangle) g.Shapes[0];
            var t2 = (SmoothTriangle) g.Shapes[1];

            t1.P1.Should().Be(parser.Vertices[1]);
            t1.P2.Should().Be(parser.Vertices[2]);
            t1.P3.Should().Be(parser.Vertices[3]);
            t1.N1.Should().Be(parser.Normals[3]);
            t1.N2.Should().Be(parser.Normals[1]);
            t1.N3.Should().Be(parser.Normals[2]);

            t2.P1.Should().Be(parser.Vertices[1]);
            t2.P2.Should().Be(parser.Vertices[2]);
            t2.P3.Should().Be(parser.Vertices[3]);
            t2.N1.Should().Be(parser.Normals[3]);
            t2.N2.Should().Be(parser.Normals[1]);
            t2.N3.Should().Be(parser.Normals[2]);
        }
    }
}
