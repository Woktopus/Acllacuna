using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace Acllacuna
{
	public class Enemy : Player
	{
		private static int NextID = 0;

		private int id;

		private DirectionEnum direction;

		private bool shouldMove;

		public Enemy() : base()
		{
			id = NextID;
			NextID++;

			direction = DirectionEnum.LEFT;
			shouldMove = true;
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
			animation.LoadContent(content, "Graphics/Spritesheet", Color.DarkRed, GetDrawPosition(), 200, new Vector2(4, 4));

			animation.SelectAnimation(0);

			animation.ScaleToMeters(size);
		}

		protected override void SetVelocity(World world)
		{
			Vector2 velocity = body.LinearVelocity;

			hasMoved = false;

			desiredHorizontalVelocity = velocity.X * 0.95f;
			if (shouldMove && direction == DirectionEnum.LEFT)
			{
				desiredHorizontalVelocity = MathHelper.Max(velocity.X - 0.5f, -5.0f);
				hasMoved = true;
				this.directionRegard = DirectionEnum.LEFT;
			}
			if (shouldMove && direction == DirectionEnum.RIGHT)
			{
				desiredHorizontalVelocity = MathHelper.Min(velocity.X + 0.5f, 5.0f);
				hasMoved = true;
				this.directionRegard = DirectionEnum.RIGHT;
			}

			float velocityChange = desiredHorizontalVelocity - velocity.X;
			float impulse = body.Mass * velocityChange;

			body.ApplyLinearImpulse(new Vector2(impulse, 0), body.WorldCenter);
		}
		
	}
}
