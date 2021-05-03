using System.Collections.Generic;
using System.Globalization;
using TheRayTracerChallenge.Shapes;
using System.Linq;
using System.IO;
using TheRayTracerChallenge.Tests.ObjFileParsing;
using System;

namespace TheRayTracerChallenge.ObjFileParsing
{
    class ObjFileParser
    {
        private Dictionary<string, Group> _groups = new Dictionary<string, Group>();

        public ObjFileParser()
        {
            // Dummies to get 1-indexed arrays.
            Vertices.Add(Tuple.Point(0, 0, 0));
            Normals.Add(Tuple.Vector(0, 0, 0));

            // Add the default group
            _groups.Add("", new Group());
        }

        public List<Tuple> Vertices { get; } = new List<Tuple>();
        public List<Tuple> Normals { get; } = new List<Tuple>();

        public int NumberIgnoredLines { get; private set; }

        public Group DefaultGroup => _groups[""];

        public Group BundlingGroup
            => _groups.Count == 1 
            ? DefaultGroup 
            : new Group(_groups.Values.Where(group => group.Shapes.Any())); // Remove empty groups, if any.

        public static ObjFileParser ParseFromFile(string path)
        {
            var content = File.ReadAllText(path);
            return Parse(content);
        }

        public static ObjFileParser Parse(string content)
        {
            var parser = new ObjFileParser();
            parser.InternalParse(content);
            Console.WriteLine($"Bundling group bounds {parser.BundlingGroup.Bounds}");
            return parser;
        }

        private void InternalParse(string content)
        {
            var currentGroup = _groups[""];
            var lines = content.Replace("\r\n", "\n").Split("\n");
            foreach (var line in lines)
            {
                var lineType = GetLineType(line);
                switch(lineType)
                {
                    case LineType.Vertice:
                        Vertices.Add(ParseVertice(line));
                        break;
                    case LineType.Triangle:
                        currentGroup.AddChild(ParseTriangle(line));
                        break;
                    case LineType.SmoothTriangle:
                        currentGroup.AddChild(ParseSmoothTriangle(line));
                        break;
                    case LineType.Polygon:
                        currentGroup.AddChilds(ParseTriangles(line));
                        break;
                    case LineType.SmoothPolygon:
                        currentGroup.AddChilds(ParseSmoothTriangles(line));
                        break;
                    case LineType.GroupName:                        
                        currentGroup = new Group();
                        var groupName = ParseGroupName(line);
                        _groups.Add(groupName, currentGroup);
                        break;
                    case LineType.VertexNormal:
                        Normals.Add(ParseVertexNormal(line));
                        break;
                    default:
                        NumberIgnoredLines++;
                        break;
                };
            }
        }

        private LineType GetLineType(string line)
        {
            if (line.StartsWith("v "))
                return LineType.Vertice;

            if (line.StartsWith("g "))
                return LineType.GroupName;

            if (line.StartsWith("f "))
            {
                var numberSpaces = line.Count(c => c == ' ');
                if (numberSpaces == 3)
                    return line.Contains("/") ? LineType.SmoothTriangle : LineType.Triangle;
                else if (numberSpaces > 3)
                    return line.Contains("/") ? LineType.SmoothPolygon : LineType.Polygon;
            }

            if (line.StartsWith("vn "))
                return LineType.VertexNormal;
                
            return LineType.Unknown;
        }

        private static Tuple ParseVertice(string line)
        {
            // For example: "v -1.0000 0.5000 0.0000"
            // Can begin with a v followed by two spaces "v  "
            var points = line.Split(" ", System.StringSplitOptions.RemoveEmptyEntries)
                .Skip(1) // The "v"
                .Select(doubleStr => double.Parse(doubleStr, CultureInfo.InvariantCulture))
                .ToList();
            return Tuple.Point(points[0], points[1], points[2]);
        }

        private Triangle ParseTriangle(string line)
        {
            //For example: "f 1 2 3"
            var vertices = line.Split(" ")
                .Skip(1) // The "f"
                .Select(indexStr => int.Parse(indexStr))
                .Select(index => Vertices[index])
                .ToList();
            return new Triangle(vertices[0], vertices[1], vertices[2]);
        }

        private Shape ParseSmoothTriangle(string line)
        {
            // For example "f 1/0/3 2/102/1 3/14/2"
            var verticeInfos = line.Split(" ")
                .Skip(1) // The "f"
                .Select(viInfo =>
                {
                    var indices = viInfo.Split("/");
                    return new { 
                        Vertex = Vertices[int.Parse(indices[0])],
                        Normal = Normals[int.Parse(indices[2])]
                    };
                })
                .ToList();
            return new SmoothTriangle(
                verticeInfos[0].Vertex, verticeInfos[1].Vertex, verticeInfos[2].Vertex, 
                verticeInfos[0].Normal, verticeInfos[1].Normal, verticeInfos[2].Normal);
        }

        internal Group GetGroup(string name)
            => _groups[name];
        
        private List<Triangle> ParseTriangles(string line)
        {
            //For example: "f 1 2 3 4 5"
            var vertices = line.Split(" ")
                .Skip(1) // The "f"
                .Select(indexStr => int.Parse(indexStr))
                .Select(index => Vertices[index])
                .ToList();

            // Assume convex polygons only -> can use "fan triangulation"
            return Enumerable
                .Range(1, vertices.Count - 2)
                .Select(index => new Triangle(vertices[0], vertices[index], vertices[index + 1]))
                .ToList();
        }

        private IEnumerable<Shape> ParseSmoothTriangles(string line)
        {
            //For example "f 75/75/75 110/110/110 108/108/108 73/73/73"
            var verticeInfos = line.Split(" ", System.StringSplitOptions.RemoveEmptyEntries)
                .Skip(1) // The "f"
                .Select(viInfo =>
                {
                    var indices = viInfo.Split("/");
                    return new
                    {
                        Vertex = Vertices[int.Parse(indices[0])],
                        Normal = Normals[int.Parse(indices[2])]
                    };
                })
                .ToList();

            // Assume convex polygons only -> can use "fan triangulation"
            return Enumerable
                .Range(1, verticeInfos.Count - 2)
                .Select(index => new SmoothTriangle(
                    verticeInfos[0].Vertex, verticeInfos[index].Vertex, verticeInfos[index+1].Vertex,
                    verticeInfos[0].Normal, verticeInfos[index].Normal, verticeInfos[index+1].Normal))
                .ToList();
        }

        private string ParseGroupName(string line)
            => line.Split(" ")[1];

        private Tuple ParseVertexNormal(string line)
        {
            var values = line.Split(" ")
                .Skip(1) // The "vn"
                .Select(doubleStr => double.Parse(doubleStr, CultureInfo.InvariantCulture))
                .ToList();
            return Tuple.Vector(values[0], values[1], values[2]);
        }
    }
}
