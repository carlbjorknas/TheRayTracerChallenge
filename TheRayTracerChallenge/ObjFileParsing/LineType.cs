using System;
using System.Collections.Generic;
using System.Text;

namespace TheRayTracerChallenge.ObjFileParsing
{
    enum LineType
    {
        Unknown,
        Vertice,
        Triangle,
        Polygon,
        GroupName,
        VertexNormal,
        SmoothTriangle,
        SmoothPolygon
    }
}
