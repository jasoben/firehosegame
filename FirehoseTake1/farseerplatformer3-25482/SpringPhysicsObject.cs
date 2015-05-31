using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics.Joints;

namespace FarseerPlatformer
{

    public class SpringPhysicsObject : CompositePhysicsObject
    {
        protected AngleJoint springJoint;

        public SpringPhysicsObject(World world, PhysicsObject physObA, PhysicsObject physObB, Vector2 relativeJointPosition, float springSoftness, float springBiasFactor)
            : base(world, physObA, physObB, relativeJointPosition)
        {
            springJoint = JointFactory.CreateAngleJoint(world, physObA.fixture.Body, physObB.fixture.Body);
            springJoint.TargetAngle = 0;
            springJoint.Softness = springSoftness;
            springJoint.BiasFactor = springBiasFactor;
        }
    }
}