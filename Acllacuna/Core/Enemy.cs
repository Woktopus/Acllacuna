using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
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
            Health = 50;

			direction = DirectionEnum.LEFT;
			shouldMove = true;

			sensors = new Fixture[4];
			sensorsContacts = new int[4];
			sensorsContacts[0] = 0;
			sensorsContacts[1] = 0;
			sensorsContacts[2] = 0;
			sensorsContacts[3] = 0;

			contactsWithFloor = 1;
		}

		public override void SetSize()
		{
			this.size = new Vector2(2.5f, 4f);
			this.sizeRatio = new Vector2(0.7f, 0.9f);
		}

		public override void SetIDS()
		{
			body.FixtureList[0].UserData = (int)(100 + id);
			feet[0].UserData = (int)(100 + id);
			feet[1].UserData = (int)(100 + id);
			feet[2].UserData = (int)(100 + id);
			bumpers[0].UserData = (int)(100 + id);
			bumpers[1].UserData = (int)(100 + id);
			bumpers[2].UserData = (int)(100 + id);
			bumpers[3].UserData = (int)(100 + id);
			bumpers[4].UserData = (int)(100 + id);
			bumpers[5].UserData = (int)(100 + id);
			bumpers[6].UserData = (int)(100 + id);
			bumpers[7].UserData = (int)(100 + id);
			bumpers[8].UserData = (int)(100 + id);
			bumpers[9].UserData = (int)(100 + id);
		}

		public override void LoadAnimation(ContentManager content)
		{
			animation.LoadContent(content, "Graphics/redsheet", Color.White, GetDrawPosition(), 200, new Vector2(3, 4));

			animation.SelectAnimation(0);

			animation.ScaleToMeters(size);

			animation.isActive = true;
		}

		public override void LoadContent(World world, ContentManager content, Vector2 position, PhysicsScene physicsScene)
		{
			base.LoadContent(world, content, position, physicsScene);

			Vector2 size = this.size * sizeRatio;

			CircleShape circle1 = new CircleShape(0.1f, 1f);
			CircleShape circle2 = new CircleShape(0.1f, 1f);
			CircleShape circle3 = new CircleShape(0.1f, 1f);
			CircleShape circle4 = new CircleShape(0.1f, 1f);

			circle1.Position = new Vector2(-((size.X / 2) + 0.2f), (size.Y + 0.1f) / 2);
			circle2.Position = new Vector2((size.X / 2) + 0.2f, (size.Y + 0.1f) / 2);
			circle3.Position = new Vector2(-((size.X / 2) + 0.2f), (size.Y - 0.3f) / 2);
			circle4.Position = new Vector2((size.X / 2) + 0.2f, (size.Y - 0.3f) / 2);

			sensors[0] = body.CreateFixture(circle1, (int)-(100 + id));
			sensors[1] = body.CreateFixture(circle2, (int)-(200 + id));
			sensors[2] = body.CreateFixture(circle3, (int)-(300 + id));
			sensors[3] = body.CreateFixture(circle4, (int)-(400 + id));

			sensors[0].IsSensor = true;
			sensors[1].IsSensor = true;
			sensors[2].IsSensor = true;
			sensors[3].IsSensor = true;
		}

		public override void SetVelocity(World world, GameTime gameTime)
		{
			if (sensorsContacts[0] == 0 || sensorsContacts[2] > 0)
			{
				direction = DirectionEnum.RIGHT;
			}
			else if (sensorsContacts[1] == 0 || sensorsContacts[3] > 0)
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
		
        public void Update(GameTime gameTime, World world)
        {
            feet[0].Friction = 1000;
            feet[1].Friction = 1000;
            feet[2].Friction = 1000;

            SetVelocity(world, gameTime);

            if (isDamaged)
            {
                isDamaged = false;
                float impulse;

                if (directionRegard == DirectionEnum.LEFT)
                {
                    float velocityChange = 8 - body.LinearVelocity.X;
                    impulse = body.Mass * velocityChange;
                }
                else
                {
                    float velocityChange = -8 - body.LinearVelocity.X;
                    impulse = body.Mass * velocityChange;
                }

                body.ApplyLinearImpulse(new Vector2(impulse, 0), body.WorldCenter);
                float jumpVelocity = PhysicsUtils.GetVerticalSpeedToReach(world, 4);
                body.LinearVelocity = new Vector2(body.LinearVelocity.X, -jumpVelocity);

                animation.SelectAnimation(3);
                animation.loop = false;
            }
            else
            {
                if (hasMoved && animation.isEnded && contactsWithFloor > 0)
                {
                    animation.SelectAnimation(1);
                    animation.loop = false;
                }

                if (hasJumped)
                {
                    hasJumped = false;
                    animation.SelectAnimation(2);
                    animation.loop = false;
                }

            }

            if (hasMoved)
            {
                feet[0].Friction = 0;
                feet[1].Friction = 0;
                feet[2].Friction = 0;
            }

            for (ContactEdge contactEdge = body.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
            {
                contactEdge.Contact.ResetFriction();
            }

            animation.position = GetDrawPosition();
            animation.Update(gameTime);
            dagger.bodyPosition = this.GetPositionFromBody();
            dagger.Update(gameTime);
        }

	}
}
