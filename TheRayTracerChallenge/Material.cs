﻿using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Patterns;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge
{
    internal class Material
    {
        public Material()
        {
            Color = new Color(1, 1, 1);
            Ambient = 0.1;
            Diffuse = 0.9;
            Specular = 0.9;
            Shininess = 200;
            RefractiveIndex = 1.0;
        }

        public Color Color { get; internal set; }
        public double Ambient { get; internal set; }
        public double Diffuse { get; internal set; }
        public double Specular { get; internal set; }
        public double Shininess { get; internal set; }
        public Pattern Pattern { get; internal set; }
        public double Reflective { get; internal set; }
        public double Transparency { get; internal set; }
        public double RefractiveIndex { get; internal set; }

        public override bool Equals(object obj)
        {
            return obj is Material m &&
                Color.Equals(m.Color) &&
                Ambient == m.Ambient &&
                Diffuse == m.Diffuse &&
                Specular == m.Specular &&
                Shininess == m.Shininess;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Color, Ambient, Diffuse, Specular, Shininess);
        }

        internal Color Lighting(Shape shape, PointLight light, Tuple point, Tuple eyev, Tuple normalv, bool inShadow)
        {
            var color = Pattern == null 
                ? Color 
                : Pattern.PatternColorAtShape(shape, point);

            var effectiveColor = color * light.Intensity;
            var lightv = (light.Position - point).Normalize;
            var ambient = effectiveColor * Ambient;

            if (inShadow)
                return ambient;

            var lightDotNormal = lightv.Dot(normalv);
            if (lightDotNormal < 0)
                return ambient;

            var diffuse = effectiveColor * Diffuse * lightDotNormal;
            var reflectv = -lightv.Reflect(normalv);
            var reflectDotEye = reflectv.Dot(eyev);
            var specular = Color.Black;
            if (reflectDotEye > 0)
            {
                var factor = Math.Pow(reflectDotEye, Shininess);
                specular = light.Intensity * Specular * factor;
            }

            return ambient + diffuse + specular;
        }
    }
}
