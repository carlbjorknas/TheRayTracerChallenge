using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.ObjFileParsing;

namespace TheRayTracerChallenge.Scenes
{
    class TreeFromObjFileScene : SceneBase
    {
        public TreeFromObjFileScene(string filename) : base(filename)
        {
        }

        protected override Tuple CameraFromPoint => Tuple.Point(1, 0, -4);
        protected override Tuple CameraToPoint => Tuple.Point(0, 0, 0);

        protected override void CreateScenery()
        {
            // Bundling group bounds Min: -1,162461, -1,895607, -1,119851 Max: 1,143143, 3,227034, 1,15625
            var parser = ObjFileParser.ParseFromFile("ObjFileParsing\\ObjFiles\\lowpolytree.obj");
            AddShapesToWorld(parser.BundlingGroup);
        }
    }
}
