﻿using FarseerPhysics;
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
	public class Player
	{
		protected Body body;

		protected Fixture[] feet;

		protected Image image;

		protected Vector2 size;

		public int contactsWithFloor;

		protected float desiredHorizontalVelocity;

        //Stats
        public int Health { get; set; }
        public int Ammo { get; set; }

		public Player()
		{
			image = new Image();

			contactsWithFloor = 0;

            Health = 50;

			feet = new Fixture[3];
		}

		public Vector2 GetPositionFromBody()
		{
			return body.Position;
		}

		public void LoadContent(World world, ContentManager content, Vector2 position)
		{
			SetSize();

			body = BodyFactory.CreateRectangle(world, size.X, size.Y - 0.1f, 1f);
			
			body.BodyType = BodyType.Dynamic;

			body.FixedRotation = true;

			body.Position = position;

			body.Mass = 50;

			CircleShape circle1 = new CircleShape(0.1f, 1f);
			CircleShape circle2 = new CircleShape(0.1f, 1f);
			CircleShape circle3 = new CircleShape(0.1f, 1f);

			circle1.Position = new Vector2(-((size.X/ 2) - 0.1f), (size.Y - 0.1f) / 2);
			circle2.Position = new Vector2(0, (size.Y - 0.1f) / 2);
			circle3.Position = new Vector2((size.X / 2) - 0.1f, (size.Y - 0.1f) / 2);

			feet[0] = body.CreateFixture(circle1);
			feet[1] = body.CreateFixture(circle2);
			feet[2] = body.CreateFixture(circle3);

			SetIDS();

			LoadAnimation(content);
		}

		protected virtual void SetSize()
		{
			this.size = new Vector2(2.5f, 3f);
		}

		protected virtual void SetIDS()
		{
			body.FixtureList[0].UserData = (int)0;
			feet[0].UserData = (int)1;
			feet[1].UserData = (int)1;
			feet[2].UserData = (int)1;
		}

		protected virtual void LoadAnimation(ContentManager content)
		{
			image.LoadContent(content, "Graphics/virgin", Color.White, GetDrawPosition());

			image.ScaleToMeters(size);
		}

		public void Update(GameTime gameTime, World world)
		{
			feet[0].Friction = 1000;
			feet[1].Friction = 1000;
			feet[2].Friction = 1000;

			SetVelocity(world);

			if (desiredHorizontalVelocity != 0)
			{
				feet[0].Friction = 0;
				feet[1].Friction = 0;
				feet[2].Friction = 0;
			}

			for (ContactEdge contactEdge = body.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
			{
				contactEdge.Contact.ResetFriction();
			}

			image.position = GetDrawPosition();
		}

		protected virtual void SetVelocity(World world)
		{
			KeyboardState keyboardInput = ServiceHelper.Get<InputManagerService>().Keyboard.GetState();

			Vector2 velocity = body.LinearVelocity;

			desiredHorizontalVelocity = 0f;
			if (keyboardInput.IsKeyDown(Keys.Left))
			{
				desiredHorizontalVelocity = MathHelper.Max(velocity.X - 0.5f, -5.0f);
			}
			if (keyboardInput.IsKeyDown(Keys.Right))
			{
				desiredHorizontalVelocity = MathHelper.Min(velocity.X + 0.5f, 5.0f);
			}

			float velocityChange = desiredHorizontalVelocity - velocity.X;
			float impulse = body.Mass * velocityChange;

			body.ApplyLinearImpulse(new Vector2(impulse, 0), body.WorldCenter);

			if (keyboardInput.IsKeyDown(Keys.Up) && contactsWithFloor > 0)
			{
				float jumpVelocity = PhysicsUtils.GetVerticalSpeedToReach(world, 2);
				body.LinearVelocity = new Vector2(velocity.X, -jumpVelocity);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			image.Draw(spriteBatch);
		}

		protected Vector2 GetDrawPosition()
		{
			return ConvertUnits.ToDisplayUnits(body.Position + new Vector2(0, 0.1f / 2));
		}
	}
}
