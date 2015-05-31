using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics.Joints;

namespace FarseerPlatformer
{
    public class CompositePhysicsObject
    {
        protected PhysicsObject physObA, physObB;
        protected RevoluteJoint revJoint;

        public CompositePhysicsObject(World world, PhysicsObject physObA, PhysicsObject physObB, Vector2 relativeJointPosition)
        {
            this.physObA = physObA;
            this.physObB = physObB;
            revJoint = JointFactory.CreateRevoluteJoint(world, physObA.fixture.Body, physObB.fixture.Body, ConvertUnits.ToSimUnits(relativeJointPosition));
            physObA.fixture.CollisionFilter.IgnoreCollisionWith(physObB.fixture);
            physObB.fixture.CollisionFilter.IgnoreCollisionWith(physObA.fixture);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            physObA.Draw(spriteBatch);
            physObB.Draw(spriteBatch);
        }

    }
}