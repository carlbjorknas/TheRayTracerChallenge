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

            if (line.StartsWith("f ") && line.Count(c => c == ' ') == 3)
                return LineType.Triangle;

            return LineType.Unknown;
        }

        private Triangle ParseTriangle(string line)
        {
            //For example: "f 1 2 3"
            var parts = line.Split(" ");
            return new Triangle(
                GetVertice(parts[1]), 
                GetVertice(parts[2]), 
                GetVertice(parts[3]));

            Tuple GetVertice(string verticeIndexStr)
            {
                var verticeIndex = int.Parse(verticeIndexStr);
                return Vertices[verticeIndex];
            }
        }

        public static Tuple ParseVertice(string line)
        {
            // For example: "v -1.0000 0.5000 0.0000"
            var parts = line.Split(" ");
            return Tuple.Point(
                double.Parse(parts[1], CultureInfo.InvariantCulture),
                double.Parse(parts[2], CultureInfo.InvariantCulture),
                double.Parse(parts[3], CultureInfo.InvariantCulture));

        }
    }
}
