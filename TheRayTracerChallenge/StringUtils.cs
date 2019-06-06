using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge
{
    static class StringUtils
    {
        public static string BreakIntoLinesWithMaxLength(this string str, int maxLength)
        {
            if (str.Length <= maxLength)
                return str;

            var index = str.LastIndexOf(' ', maxLength);
            var shortenedStr = str.Substring(0, index);
            var restStr = str.Substring(index + 1);

            return shortenedStr + "\r\n" + restStr.BreakIntoLinesWithMaxLength(maxLength);
        }
    }
}
