using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acllacuna
{
	public class Boss
	{
		public int health;

		public Body body;

		public Image image;

		public Fixture head;

		public Fixture[] hands;

		public bool isDead;

		public Boss()
		{
			health = 300;
			image = new Image();
			hands = new Fixture[2];
			isDead = false;
		}

		public void LoadContent(ContentManager content, World world, Vector2 position, Vector2 size)
		{
			body = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);

			body.FixedRotation = true;

			body.FixtureList[0].UserData = 9000;

			body.Position = ConvertUnits.ToSimUnits(position);

			body.BodyType = BodyType.Dynamic;

            CircleShape circle1 = new CircleShape(size.X / 4, 1f);
            CircleShape circle2 = new CircleShape(size.Y / 6, 1f);
            CircleShape circle3 = new CircleShape(size.Y / 6, 1f);

            circle1.Position = new Vector2(0, -size.Y / 3);
            circle2.Position = new Vector2(-1.5f * size.X / 5, -size.Y / 10);
			circle3.Position = new Vector2(-0.4f * size.X / 5, -size.Y / 10);

			head = body.CreateFixture(circle1);
			hands[0] = body.CreateFixture(circle2);
			hands[1] = body.CreateFixture(circle3);

			head.UserData = (int)10000;
			hands[0].UserData = (int)11000;
			hands[1].UserData = (int)11000;

			image.LoadContent(content, "Graphics/tezka", Color.White, position);

			image.ScaleToMeters(size);

			image.position = ConvertUnits.ToDisplayUnits(body.Position);
		}

		public void Update(GameTime gametime)
		{
			image.position = ConvertUnits.ToDisplayUnits(body.Position);
		}

		public void Draw(SpriteBatch spritebatch)
		{
			if (isDead) return;

			image.Draw(spritebatch);
		}
	}
}
