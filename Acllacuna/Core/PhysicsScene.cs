﻿using System;
using System.Collections.Generic;
using System.Linq;

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

        Player player;

        public List<CollectibleItem> collectibleItems { get; set; }

		public List<Enemy> enemies;

        public Map map;

		public PhysicsScene()
		{

			world = null;
			gravity = new Vector2(0, 20);
            


			player = new Player();

            collectibleItems = new List<CollectibleItem>();

			enemies = new List<Enemy>();
		
            map = new Map("");

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

			player.LoadContent(world, content, new Vector2(10, 0));


            map.LoadContent(content, world);

            CollectibleItem item = new CollectibleItem();
            item.LoadContent(world, new Vector2(1, 1), new Vector2(21, 8), content, "Graphics/Collectible/plume", CollectibleItemType.HEALTH);
            collectibleItems.Add(item);

			Enemy enemy = new Enemy();
			enemy.LoadContent(world, content, new Vector2(12, 0));
			enemies.Add(enemy);
        }

		bool onBeginContact( Contact contact )
		{
            BeginContactForCollectibleItem(contact);
			BeginContactForPlayer(contact);
            return true;
        }

		

		void onEndContact( Contact contact )
		{
			EndContactForPlayer(contact);
		}

        private void BeginContactForCollectibleItem(Contact contact)
        {
            Fixture fixtureA = contact.FixtureA;
            Fixture fixtureB = contact.FixtureB;

            if ((int)fixtureA.UserData >= 500 && (int)fixtureA.UserData<600
                && (int)fixtureB.UserData == 0)
            {
                int collectibleItemId = (int)fixtureA.UserData - 500;
                CollectibleItem item = collectibleItems
                    .FirstOrDefault(i => i.id == collectibleItemId);
                
                
                if (item.type == CollectibleItemType.HEALTH)
                {
                    //Ajouter santé player ici
                    if (player.Health < 100)
                    {
                        player.Health += 20;
                        collectibleItems.Remove(item);
                    }
                    
                }
                if (item.type == CollectibleItemType.AMMO)
                {
                    //Ajouter munition ici
                }
            }
            if ((int)fixtureB.UserData >= 500 && (int)fixtureB.UserData<600
                && (int)fixtureA.UserData == 0)
            {
                int collectibleItemId = (int)fixtureB.UserData - 500;
                CollectibleItem item = collectibleItems
                    .FirstOrDefault(i => i.id == collectibleItemId);


                if (item.type == CollectibleItemType.HEALTH)
                {
                    //Ajouter santé player ici
                    if (player.Health < 100)
                    {
                        player.Health += 20;
                        item.body.Dispose();
                        collectibleItems.Remove(item);
                    }

                }
                if (item.type == CollectibleItemType.AMMO)
                {
                    //Ajouter munition ici
                }
            }

        }

		private void BeginContactForPlayer(Contact contact)
		{
			Fixture fixtureA = contact.FixtureA;
			Fixture fixtureB = contact.FixtureB;

			if ((int)fixtureA.UserData == 1
				&& ((int)fixtureB.UserData == 2
				|| (int)fixtureB.UserData == 3))
			{
				player.contactsWithFloor++;
			}
			if ((int)fixtureB.UserData == 1
				&& ((int)fixtureA.UserData == 2
				|| (int)fixtureA.UserData == 3))
			{
				player.contactsWithFloor++;
			}
		}
    
            
		private void EndContactForPlayer(Contact contact)
		{
			Fixture fixtureA = contact.FixtureA;
			Fixture fixtureB = contact.FixtureB;

			if ((int)fixtureA.UserData == 1
				&& ((int)fixtureB.UserData == 2
				|| (int)fixtureB.UserData == 3))
			{
				player.contactsWithFloor--;
			}

			if ((int)fixtureB.UserData == 1
				&& ((int)fixtureA.UserData == 2
				|| (int)fixtureA.UserData == 3))
			{
				player.contactsWithFloor--;
			}
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

		public override void Update(GameTime gameTime, Game game)
		{
			base.Update(gameTime, game);
            
            // variable time step but never less then 30 Hz
			world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / PhysicsUtils.FPS)));
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
            foreach (CollectibleItem item in collectibleItems)
            {
                item.Draw(spriteBatch);
			}

			foreach (Enemy enemy in enemies)
			{
				enemy.Draw(spriteBatch);
			}

			player.Draw(spriteBatch);

            map.Draw(spriteBatch);
            
            debugView.RenderDebugData(ref projection);
		}
	}
}
