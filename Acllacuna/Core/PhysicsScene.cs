using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.DebugView;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Contacts;

namespace Acllacuna
{
	class PhysicsScene : Scene
	{
		protected World world;

		protected Vector2 gravity;

		protected DebugViewXNA debugView;
		protected Matrix projection;

		List<Body> platforms;
		List<Image> platformsImages;

		public PhysicsScene()
		{
			world = null;

			gravity = new Vector2(0, 10);

			platforms = new List<Body>();
			platformsImages = new List<Image>();
		}

		public override void LoadContent(ContentManager content, GraphicsDevice graph)
		{
			base.LoadContent(content, graph);

			Settings.UseFPECollisionCategories = true;

			ConvertUnits.SetDisplayUnitToSimUnitRatio(32f);

			if (world == null)
			{
				world = new World(Vector2.Zero);
			}
			else
			{
				world.Clear();
			}
			
			// register
			//world.ContactManager.BeginContact += onBeginContact;
			world.ContactManager.EndContact += onEndContact;
			world.ContactManager.PreSolve += onPreSolve;
			world.ContactManager.PostSolve += onPostSolve;

			world.Gravity = gravity;

			// NOTE: you should probably unregister on destructor or wherever is relevant...

			if (debugView == null)
			{
				debugView = new DebugViewXNA(world);

				debugView.LoadContent(graph, content);
			}
			
			projection = Matrix.CreateOrthographicOffCenter(
				0f, ConvertUnits.ToSimUnits(graph.Viewport.Width),
				ConvertUnits.ToSimUnits(graph.Viewport.Height), 0f,
				0f, 1f
			);

			///////////////////////////////////////////////////////////////////////////////

			for (int i = 0; i < 5; ++i)
			{
				Body platform = BodyFactory.CreateRectangle(world, 1f, 1f, 1f);

				platform.CollisionCategories = Category.Cat1;
				platform.CollidesWith = Category.Cat2;

				platform.BodyType = BodyType.Static;
				platform.Position = new Vector2(2 + (i * 3), 10);

				Image platformImage = new Image();
				platformImage.LoadContent(
					 content,
					 "Graphics/minecraft", Color.White,
					 ConvertUnits.ToDisplayUnits(platform.Position)
				 );

				platformImage.ScaleToAABB(PhysicsUtils.GetAABBFromBody(platform));

				platforms.Add(platform);
				platformsImages.Add(platformImage);
			}
		}

		/*bool onBeginContact( Contact contact )
		{
			Fixture fixtureA = contact.FixtureA;
			Fixture fixtureB = contact.FixtureB;

			Body bodyA = fixtureA.Body;
			Body bodyB = fixtureB.Body;

			BodyData dataA = (BodyData)bodyA.UserData;
			BodyData dataB = (BodyData)bodyB.UserData;

			if (dataA.name == "player" && dataB.name == "platform")
			{
				if (bodyA.Position.Y + (dataA.size.Y / 2) > bodyB.Position.Y - (dataB.size.Y / 2))
				{
					return false;
				}
			}

			if (dataB.name == "player" && dataA.name == "platform")
			{
				if (bodyB.Position.Y + (dataB.size.Y / 2) > bodyA.Position.Y - (dataA.size.Y / 2))
				{
					return false;
				}
			}

			return true;
		}*/

		void onEndContact( Contact contact )
		{
			// ...
		}

		void onPreSolve( Contact contact, ref Manifold oldManifold )
		{
			// ...
		}
		void onPostSolve( Contact contact, ContactVelocityConstraint impulse )
		{
			// ...
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// variable time step but never less then 30 Hz
			world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			debugView.RenderDebugData(ref projection);

			int platformCount = platforms.Count;
			for (int i = 0; i < platformCount; ++i)
			{
				platformsImages[i].Draw(spriteBatch);
			}
		}
	}
}
