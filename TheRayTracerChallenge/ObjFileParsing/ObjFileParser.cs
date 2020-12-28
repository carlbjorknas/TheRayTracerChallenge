using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.ObjFileParsing
{
    class ObjFileParser
    {
        public static ParseResult Parse(string content)
        {
            var lines = content.Split("\r\n");
            return new ParseResult(lines.Length);
        }
    }
}
