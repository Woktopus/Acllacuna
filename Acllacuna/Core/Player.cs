using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Acllacuna
{
	class Player
	{
		Body body;

		public Player()
		{
		}

		public void LoadContent(World world, Vector2 position)
		{
			Vector2 size = new Vector2(0.5f, 1.5f);

			body = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);

			body.BodyType = BodyType.Dynamic;

			body.FixedRotation = true;

			body.Position = position;

			body.Mass = 50;

			CircleShape circle = new CircleShape(size.X / 2, 1f);

			circle.Position = new Vector2(0, size.Y / 2);

			body.CreateFixture(circle);
		}

		public void Update(GameTime gameTime, World world)
		{
			KeyboardState keyboardInput = ServiceHelper.Get<InputManagerService>().Keyboard.GetState();

			Vector2 velocity = body.LinearVelocity;

			float desiredVelocity = 0f;
			if (keyboardInput.IsKeyDown(Keys.Left))
			{
				desiredVelocity = MathHelper.Max(velocity.X - 0.5f, -5.0f);
			}
			if (keyboardInput.IsKeyDown(Keys.Right))
			{
				desiredVelocity = MathHelper.Min(velocity.X + 0.5f, 5.0f);
			}

			float velocityChange = desiredVelocity - velocity.X;
			float impulse = body.Mass * velocityChange;

			body.ApplyLinearImpulse(new Vector2(impulse, 0), body.WorldCenter);

			if (keyboardInput.IsKeyDown(Keys.Up))
			{
				float jumpVelocity = PhysicsUtils.GetVerticalSpeedToReach(world, 2);
				body.LinearVelocity = new Vector2(velocity.X, -jumpVelocity);
			}
		}
	}
}
