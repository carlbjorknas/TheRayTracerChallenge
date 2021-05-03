using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.Scenes
{
    abstract class SceneBase
    {
        private readonly Camera _camera;
        private readonly string _filename;
        private readonly World _world;

        public SceneBase(string filename)
        {
            _camera = new Camera(CameraHSize, CameraVSize, CameraFieldOfView);
            _camera.Transform = Transformation.ViewTransform(CameraFromPoint, CameraToPoint, CameraUp);
            _filename = filename + ".ppm";
            
            _world = new World();
            _world.LightSource = new PointLight(Tuple.Point(-10, 10, -10), Color.White);
        }

        public void Render()
        {
            CreateScenery();
            var canvas = _camera.Render(_world);
            SaveImage(_filename, canvas.ToPpm());
        }

        protected abstract void CreateScenery();
        protected virtual int CameraHSize => 400;
        protected virtual int CameraVSize => 400;
        protected virtual double CameraFieldOfView => Math.PI / 3;
        protected virtual Tuple CameraFromPoint => Tuple.Point(0, 0, 0);
        protected virtual Tuple CameraToPoint => Tuple.Point(0, 0, 1);
        protected virtual Tuple CameraUp { get; } = Tuple.Point(0, 1, 0);

        protected void AddShapesToWorld(params Shape[] shapes)
        {
            _world.Shapes.AddRange(shapes);
        }

        private static void SaveImage(string name, string ppm)
        {
            File.WriteAllText("..\\..\\..\\Images\\" + name, ppm);
        }
    }
}
