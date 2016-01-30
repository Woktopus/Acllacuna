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

		public PhysicsScene()
		{
			world = null;

			gravity = new Vector2(0, 20);
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
			world.ContactManager.BeginContact += onBeginContact;
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
		}

		bool onBeginContact( Contact contact )
		{
			// ...
			return true;
		}

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
			world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / PhysicsUtils.FPS)));
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			debugView.RenderDebugData(ref projection);
		}
	}
}
