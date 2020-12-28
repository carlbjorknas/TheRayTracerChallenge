using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.ObjFileParsing
{
    internal class ParseResult
    {
        public ParseResult(int numberIgnoredLines, Tuple[] vertices, Group group)
        {
            NumberIgnoredLines = numberIgnoredLines;
            Vertices = vertices;
            Group = group;
        }

        public int NumberIgnoredLines { get; }
        public Tuple[] Vertices { get; }
        public Group Group { get; }
    }
}