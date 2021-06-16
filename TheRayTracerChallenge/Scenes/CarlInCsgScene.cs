using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Text;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.Scenes
{
    class CarlInCsgScene : SceneBase
    {
        public CarlInCsgScene():base("CsgCarl")
        {
        }

        protected override Tuple CameraFromPoint => Tuple.Point(1, 1, -2);
        protected override Tuple CameraToPoint => Tuple.Point(0, 0, 0);

        protected override void CreateScenery()
        {
            //var c = CreateC();
            //AddShapesToWorld(c);

            var a = CreateA();
            //AddShapesToWorld(a);

            var background = new Plane();
            background.Transform = Transformation.Translation(0, 0, 5)
               .Chain(Transformation.RotationX(Math.PI / 2));
            background.Material = new Material
            {
                Color = new Color(0, 0, 0.9)
            };
            AddShapesToWorld(background);
        }

        private Shape CreateC()
        {
            var sphere = Sphere.UnitSphere();
            sphere.Transform = Transformation.Scaling(1, 0.7, 1);
            var frontCube = new Cube();
            frontCube.Transform = Transformation.Translation(0, 0, -1);
            var frontShavedOff = new Csg(CsgOperation.Difference, sphere, frontCube);

            var rightCube = new Cube();
            rightCube.Transform = Transformation.Translation(1, 0, 0);
            var rightSideShavedOff = new Csg(CsgOperation.Difference, frontShavedOff, rightCube);

            var shot = new Cylinder(-1, 1, true);
            shot.Transform = Transformation.Translation(0.3, 0, 0)
                .Chain(Transformation.Scaling(1, 0.5, 1))
                .Chain(Transformation.RotationX(Math.PI / 2));
            return new Csg(CsgOperation.Difference, rightSideShavedOff, shot);
        }

        private Shape CreateA()
        {
            var trianglePrism = CreateTrianglePrism();
            trianglePrism.Transform = Transformation.Scaling(0.5, 1, 0.3);

            var trianglePrismHole = CreateTrianglePrism();
            trianglePrismHole.Transform = Transformation.Translation(0, 0.7, 0)
                .Chain(Transformation.Scaling(0.3, 0.3, 1))
                .Chain(Transformation.Scaling(0.5, 1, 2))
                .Chain(Transformation.Translation(0, 0, -0.1));
            //AddShapesToWorld(trianglePrismHole);
            var withHole = new Csg(CsgOperation.Difference, trianglePrism, trianglePrismHole);

            var bottomHole = CreateBottomHoleForA();
            //AddShapesToWorld(bottomHole);
            var withBothHoles = new Csg(CsgOperation.Difference, withHole, bottomHole);

            withBothHoles.Transform = Transformation.Translation(0, -0.5, 0);

            return withBothHoles;
        }

        private Shape CreateBottomHoleForA()
        {
            var leftTriangle = CreateTrianglePrism();
            leftTriangle.Transform =  Transformation.Translation(-0.42, 0, 0)
                .Chain(Transformation.Scaling(0.2, 0.2, 1))
                .Chain(Transformation.Scaling(0.5, 1, 1))
                .Chain(Transformation.Translation(0, -0.01, -0.1));

            var rightTriangle = CreateTrianglePrism();
            rightTriangle.Transform = Transformation.Translation(0.42, 0, 0)
                .Chain(Transformation.Scaling(0.2, 0.2, 1))
                .Chain(Transformation.Scaling(0.5, 1, 1))
                .Chain(Transformation.Translation(0, -0.01, -0.1));

            var middleBox = new Cube();
            middleBox.Transform = Transformation.Translation(0, -1.25, 0.9)
                .Chain(Transformation.Scaling(0.45, 1, 1));

            //AddShapesToWorld(middleBox);

            var bothTriangles = new Csg(CsgOperation.Union, leftTriangle, rightTriangle);
            var all = new Csg(CsgOperation.Union, bothTriangles, middleBox);

            AddShapesToWorld(all);

            return all;
        }

        private Shape CreateTrianglePrism()
        {
            var cube = new Cube();
            cube.Transform = Transformation.RotationZ(Math.PI / 4)
                .Chain(Transformation.Translation(0, 0, 1));

            var floor = new Cube();
            floor.Transform = Transformation.Translation(0, -1, 1)
                .Chain(Transformation.Scaling(2, 1, 1.1));

            var trianglePrism = new Csg(CsgOperation.Difference, cube, floor);
            return trianglePrism;
        }
    }
}
