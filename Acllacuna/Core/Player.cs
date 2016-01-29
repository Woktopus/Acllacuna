using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
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
			body.Position = position;

			CircleShape circle = new CircleShape(size.X / 2, 1f);

			circle.Position = new Vector2(0, size.Y / 2);

			body.CreateFixture(circle);
		}
	}
}
