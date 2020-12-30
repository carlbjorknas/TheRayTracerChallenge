using System.Collections.Generic;
using System.Globalization;
using TheRayTracerChallenge.Shapes;
using System.Linq;

namespace TheRayTracerChallenge.ObjFileParsing
{
    class ObjFileParser
    {
        private Dictionary<string, Group> _groups = new Dictionary<string, Group>();

        public ObjFileParser()
        {
            // Dummy point to get 1-indexed array.
            Vertices.Add(Tuple.Point(0, 0, 0));

            // Add the default group
            _groups.Add("", new Group());
        }

        public List<Tuple> Vertices { get; } = new List<Tuple>();

        public int NumberIgnoredLines { get; private set; }

        public Group DefaultGroup => _groups[""];

        public static ObjFileParser Parse(string content)
        {
            var parser = new ObjFileParser();
            parser.InternalParse(content);
            return parser;
        }

        public void InternalParse(string content)
        {
            var currentGroup = _groups[""];
            var lines = content.Split("\r\n");
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
                    case LineType.Polygon:
                        currentGroup.AddChilds(ParseTriangles(line));
                        break;
                    default:
                        NumberIgnoredLines++;
                        break;
                };
            }
        }

        private LineType GetLineType(string line)
        {
            if (line.StartsWith("v ") && line.Count(c => c == ' ') == 3)
                return LineType.Vertice;

            if (line.StartsWith("f "))
            {
                var numberSpaces = line.Count(c => c == ' ');
                if (numberSpaces == 3)
                    return LineType.Triangle;
                else if (numberSpaces > 3)
                    return LineType.Polygon;
            }
                
            return LineType.Unknown;
        }

        public static Tuple ParseVertice(string line)
        {
            // For example: "v -1.0000 0.5000 0.0000"
            var points = line.Split(" ")
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
    }
}
