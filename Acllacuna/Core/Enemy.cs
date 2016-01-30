using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace Acllacuna
{
	public class Enemy : Player
	{
		private static int NextID = 0;

		public int id;

		public DirectionEnum direction;

		private bool shouldMove;

		private Fixture[] sensors;

		public int[] sensorsContacts;

		public Enemy() : base()
		{
			id = NextID;
			NextID++;

			direction = DirectionEnum.RIGHT;
			shouldMove = true;

			sensors = new Fixture[2];
			sensorsContacts = new int[2];
			sensorsContacts[0] = 0;
			sensorsContacts[1] = 0;
		}

		protected override void SetSize()
		{
			this.size = new Vector2(2.5f, 4f);
		}

		protected override void SetIDS()
		{
			body.FixtureList[0].UserData = (int)100;
			feet[0].UserData = (int)(100 + id);
			feet[1].UserData = (int)(100 + id);
			feet[2].UserData = (int)(100 + id);
			bumpers[0].UserData = (int)100;
			bumpers[1].UserData = (int)100;
			bumpers[2].UserData = (int)100;
			bumpers[3].UserData = (int)100;
			bumpers[4].UserData = (int)100;
			bumpers[5].UserData = (int)100;
		}

		protected override void LoadAnimation(ContentManager content)
		{
			animation.LoadContent(content, "Graphics/Spritesheet", Color.DarkRed, GetDrawPosition(), 200, new Vector2(4, 5));

			animation.SelectAnimation(0);

			animation.ScaleToMeters(size);
		}

		public override void LoadContent(World world, ContentManager content, Vector2 position, PhysicsScene physicsScene)
		{
			base.LoadContent(world, content, position, physicsScene);

			CircleShape circle1 = new CircleShape(0.1f, 1f);
			CircleShape circle2 = new CircleShape(0.1f, 1f);
			circle1.Position = new Vector2(-((size.X / 2) + 0.2f), (size.Y + 0.1f) / 2);
			circle2.Position = new Vector2((size.X / 2) + 0.2f, (size.Y + 0.1f) / 2);
			sensors[0] = body.CreateFixture(circle1, (int)-(100 + id));
			sensors[1] = body.CreateFixture(circle2, (int)-(200 + id));
			sensors[0].IsSensor = true;
			sensors[1].IsSensor = true;
		}

		protected override void SetVelocity(World world, GameTime gameTime)
		{
			if (sensorsContacts[0] == 0)
			{
				direction = DirectionEnum.RIGHT;
			}
			else if (sensorsContacts[1] == 0)
			{
				direction = DirectionEnum.LEFT;
			}

			Vector2 velocity = body.LinearVelocity;

			hasMoved = false;

			desiredHorizontalVelocity = velocity.X * 0.95f;
			if (shouldMove && direction == DirectionEnum.LEFT)
			{
				desiredHorizontalVelocity = MathHelper.Max(velocity.X - 0.5f, -4.0f);
				hasMoved = true;
				this.directionRegard = DirectionEnum.LEFT;
			}
			if (shouldMove && direction == DirectionEnum.RIGHT)
			{
				desiredHorizontalVelocity = MathHelper.Min(velocity.X + 0.5f, 4.0f);
				hasMoved = true;
				this.directionRegard = DirectionEnum.RIGHT;
			}

			float velocityChange = desiredHorizontalVelocity - velocity.X;
			float impulse = body.Mass * velocityChange;

			body.ApplyLinearImpulse(new Vector2(impulse, 0), body.WorldCenter);
		}
		
	}
}
