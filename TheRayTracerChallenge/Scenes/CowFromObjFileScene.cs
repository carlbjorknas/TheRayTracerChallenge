﻿using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.ObjFileParsing;

namespace TheRayTracerChallenge.Scenes
{
    class CowFromObjFileScene : SceneBase
    {
        public CowFromObjFileScene(string filename) : base(filename)
        {
        }

        protected override int CameraHSize => 400;
        protected override int CameraVSize => 400;
        protected override double CameraFieldOfView => Math.PI / 3;
        protected override Tuple CameraFromPoint => Tuple.Point(0, 0, -7);
        protected override Tuple CameraToPoint => Tuple.Point(0.5, 0, 0);

        protected override void CreateScenery()
        {
            var parser = ObjFileParser.ParseFromFile("ObjFileParsing\\ObjFiles\\cow-nonormals.obj");
            AddShapesToWorld(parser.BundlingGroup);
        }
    }
}
