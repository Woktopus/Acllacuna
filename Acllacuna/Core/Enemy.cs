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

		public Enemy() : base()
		{
			id = NextID;
			NextID++;
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
		}

		protected override void LoadAnimation(ContentManager content)
		{
			animation.LoadContent(content, "Graphics/Spritesheet", Color.DarkRed, GetDrawPosition(), 200, new Vector2(4, 4));

			animation.SelectAnimation(0);

			animation.ScaleToMeters(size);
		}

		protected override void SetVelocity(World world, GameTime gameTime)
		{

		}
		
	}
}
