using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheRayTracerChallenge.Shapes;

namespace TheRayTracerChallenge.Scenes
{
    class SnowflakeScene : SceneBase
    {
        public SnowflakeScene(string filename) : base(filename)
        {
        }

        protected override int CameraHSize => 400;
        protected override int CameraVSize => 400;
        protected override double CameraFieldOfView => Math.PI / 3;
        protected override Tuple CameraFromPoint => Tuple.Point(0, 0, -15);
        protected override Tuple CameraToPoint => Tuple.Point(0, 0, 0);

        protected override void CreateScenery()
        {
            var snowflake = CreateSnowflake();
            snowflake.Transform = Transformation.Translation(40, -40, 70)
                .Chain(Transformation.RotationY(Math.PI / 5));
            //.Chain(Transformation.Scaling(0.3, 0.3, 0.3));

            var s2 = CreateSnowflake();
            s2.Transform = Transformation.Translation(-40, 40, 50)
                .Chain(Transformation.RotationY(-Math.PI / 5));
            //.Chain(Transformation.Scaling(0.2, 0.2, 0.2));

            var s3 = CreateSnowflake();
            s3.Transform = Transformation.Translation(10, 10, 5)
                .Chain(Transformation.RotationY(Math.PI / 4));
            //.Chain(Transformation.Scaling(0.8, 0.8, 0.8));

            var s4 = CreateSnowflake();
            s4.Transform = Transformation.Translation(0, 0, 90)
                .Chain(Transformation.RotationY(-Math.PI / 3));

            var s5 = CreateSnowflake();
            s5.Transform = Transformation.Translation(-10, -20, 30)
                //.Chain(Transformation.RotationY(Math.PI / 5))
                .Chain(Transformation.RotationX(Math.PI / 3));

            AddShapesToWorld(snowflake, s2, s3, s4, s5);
        }

        private Shape CreateSnowflake()
        {
            var rotationAngle = Math.PI / 3;

            var group = new Group();

            Enumerable
                .Range(0, 6)
                .Select(index => CreateSnowflakePart(index * rotationAngle))
                .ToList()
                .ForEach(part => group.AddChild(part));

            return group;
        }

        private Shape CreateSnowflakePart(double rotationAngle)
        {
            var group = new Group();
            group.Transform = Transformation.RotationZ(rotationAngle);

            var mainLeg = new Cylinder(0, 10);
            group.AddChild(mainLeg);

            var leftUpperLeg = new Cylinder(0, 4);
            leftUpperLeg.Transform = Transformation.Translation(0, 8, 0)
                .Chain(Transformation.RotationZ(Math.PI / 4))
                .Chain(Transformation.Scaling(0.3, 1, 0.3));
            group.AddChild(leftUpperLeg);

            var leftLowerLeg = new Cylinder(0, 4);
            leftLowerLeg.Transform = Transformation.Translation(0, 6, 0)
                .Chain(Transformation.RotationZ(Math.PI / 4))
                .Chain(Transformation.Scaling(0.3, 1, 0.3));
            group.AddChild(leftLowerLeg);

            var rightUpperLeg = new Cylinder(0, 4);
            rightUpperLeg.Transform = Transformation.Translation(0, 8, 0)
                .Chain(Transformation.RotationZ(-Math.PI / 4))
                .Chain(Transformation.Scaling(0.3, 1, 0.3));
            group.AddChild(rightUpperLeg);

            var rightLowerLeg = new Cylinder(0, 4);
            rightLowerLeg.Transform = Transformation.Translation(0, 6, 0)
                .Chain(Transformation.RotationZ(-Math.PI / 4))
                .Chain(Transformation.Scaling(0.3, 1, 0.3));
            group.AddChild(rightLowerLeg);

            var topCone = new Cone(-1, 0);
            topCone.Transform = Transformation.Translation(0, 11, 0);
            group.AddChild(topCone);

            return group;
        }
    }
}
