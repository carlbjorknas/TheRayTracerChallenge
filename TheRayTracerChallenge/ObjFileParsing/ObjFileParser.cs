using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Regex = System.Text.RegularExpressions;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.ObjFileParsing
{
    class ObjFileParser
    {
        public static ParseResult Parse(string content)
        {
            var numberIgnoredLines = 0;
            var vertices = new List<Tuple>();
            vertices.Add(Tuple.Point(0, 0, 0)); // Dummy point to get 1-indexed array.
            var group = new Group();

            var lines = content.Split("\r\n");
            foreach(var line in lines)
            {
                if (TryParseVertice(line, out var vertice))
                    vertices.Add(vertice);
                else if (TryParseTriangle(line, vertices, out var triangle))
                    group.AddChild(triangle);
                else
                    numberIgnoredLines++;
            }

            return new ParseResult(numberIgnoredLines, vertices.ToArray(), group);
        }

        private static bool TryParseTriangle(string line, List<Tuple> vertices, out Triangle triangle)
        {
            var match = Regex.Regex.Match(line, "^f ([0-9]+) ([0-9]+) ([0-9]+)$");
            if (match.Success)
            {
                triangle = new Triangle(GetVertice(1), GetVertice(2), GetVertice(3));
                return true;
            }

            triangle = null;
            return false;

            Tuple GetVertice(int groupIndex)
            {
                var verticeIndexStr = match.Groups[groupIndex].Value;
                var verticeIndex = int.Parse(verticeIndexStr);
                return vertices[verticeIndex];
            }
        }

        public static bool TryParseVertice(string line, out Tuple vertice)
        {
            var match = Regex.Regex.Match(line, "^v ([0-9\\.\\-]+) ([0-9\\.\\-]+) ([0-9\\.\\-]+)$");
            if (match.Success)
            {
                vertice = Tuple.Point(
                    double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture),
                    double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture),
                    double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                return true;
            }

            vertice = Tuple.Point(0, 0, 0); // Dummy
            return false;
        }
    }
}
