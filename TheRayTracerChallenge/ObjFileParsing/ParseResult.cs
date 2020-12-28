namespace TheRayTracerChallenge.ObjFileParsing
{
    internal class ParseResult
    {
        public ParseResult(int numberIgnoredLines)
        {
            NumberIgnoredLines = numberIgnoredLines;
        }

        public int NumberIgnoredLines { get; }
    }
}