using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Acllacuna
{
	class Player
	{
		Body body;

		Fixture feet;

		Image image;

		Vector2 size;

		public int contactsWithFloor;

		public Player()
		{
			image = new Image();

			size = new Vector2(2.5f, 3f);

			contactsWithFloor = 0;
		}

		public void LoadContent(World world, ContentManager content, Vector2 position)
		{
			body = BodyFactory.CreateRectangle(world, size.X, size.Y - (size.X / 2), 1f);

			body.FixtureList[0].UserData = (int)0;
			
			body.BodyType = BodyType.Dynamic;

			body.FixedRotation = true;

			body.Position = position;

			body.Mass = 50;

			CircleShape circle = new CircleShape(size.X / 2, 1f);

			circle.Position = new Vector2(0, (size.Y - (size.X / 2)) / 2);

			feet = body.CreateFixture(circle);

			feet.UserData = (int)1;

			image.LoadContent(content, "Graphics/virgin", Color.White, GetDrawPosition());

			image.ScaleToMeters(size);
		}

		public void Update(GameTime gameTime, World world)
		{
			KeyboardState keyboardInput = ServiceHelper.Get<InputManagerService>().Keyboard.GetState();

			Vector2 velocity = body.LinearVelocity;

			feet.Friction = 1000;

			float desiredVelocity = 0f;
			if (keyboardInput.IsKeyDown(Keys.Left))
			{
				desiredVelocity = MathHelper.Max(velocity.X - 0.5f, -5.0f);

				feet.Friction = 0;
			}
			if (keyboardInput.IsKeyDown(Keys.Right))
			{
				desiredVelocity = MathHelper.Min(velocity.X + 0.5f, 5.0f);

				feet.Friction = 0;
			}

			for (ContactEdge contactEdge = body.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
			{
				contactEdge.Contact.ResetFriction();
			}

			float velocityChange = desiredVelocity - velocity.X;
			float impulse = body.Mass * velocityChange;

			body.ApplyLinearImpulse(new Vector2(impulse, 0), body.WorldCenter);

			if (keyboardInput.IsKeyDown(Keys.Up) && contactsWithFloor > 0)
			{
				float jumpVelocity = PhysicsUtils.GetVerticalSpeedToReach(world, 2);
				body.LinearVelocity = new Vector2(velocity.X, -jumpVelocity);
			}

			image.position = GetDrawPosition();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			image.Draw(spriteBatch);
		}

		private Vector2 GetDrawPosition()
		{
			return ConvertUnits.ToDisplayUnits(body.Position + new Vector2(0, (size.Y / 2) / 2));
		}
	}
}
