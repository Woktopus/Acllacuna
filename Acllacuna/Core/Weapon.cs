using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace Acllacuna
{
	public class Weapon
	{
		public Body body;

		public Image image;

		protected Vector2 size;

		public Weapon()
		{
			image = new Image();
		}

		public void LoadContent(World world, ContentManager content, Vector2 position, Vector2 size)
		{
			this.size = size;

			body = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);

			body.BodyType = BodyType.Static;

			body.FixedRotation = true;

			body.Position = position;
		}
	}
}
