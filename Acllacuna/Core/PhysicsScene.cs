﻿using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Media;
using System.Media;
using System.IO;

namespace Acllacuna
{
    public class PhysicsScene : Scene
	{
		public const string wintext = "CONGRATULATIONS !";

		public World world;

        protected DebugViewXNA debugView;
        protected Matrix projection;

        Camera camera;

        Player player;

        public ProjectileFactory projectileFactory { get; set; }

        public List<CollectibleItem> collectibleItems { get; set; }
        public List<Enemy> enemies;
        public List<Projectile> projectiles { get; set; }


        public Map map;
        public DynamicMap dynMap;
        public MapBackground mapBack;

        SoundPlayer song;

		Image lifeBarFrame;
		Image lifeBar;

        Image projectileBarFrame;
        Image projectileBar;

		Boss boss;

        public PhysicsScene()
        {
            world = null;

            camera = new Camera();

            player = new Player();

            collectibleItems = new List<CollectibleItem>();

            enemies = new List<Enemy>();

            map = new Map("");
            dynMap = new DynamicMap("");
            mapBack = new MapBackground("");

            projectiles = new List<Projectile>();
            projectileFactory = new ProjectileFactory();

			lifeBarFrame = new Image();
			lifeBar = new Image();

            projectileBarFrame = new Image();
            projectileBar = new Image();
        }

        public override void LoadContent(ContentManager content, GraphicsDevice graph)
		{
			base.LoadContent(content, graph);
            song = new SoundPlayer(Path.Combine(Environment.CurrentDirectory, "Content/Song/VirginInca.wav"));
            song.Load();
            song.Play();
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

            world.Gravity = PhysicsUtils.gravity;

            camera.viewportWidth = graph.Viewport.Width;
            camera.viewportHeight = graph.Viewport.Height;
            camera.zoom = 0.85f;
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

            projectileFactory.LoadContent(this, content);

            player.LoadContent(world, content, new Vector2(18, 10), this);


            map.LoadContent(content, world);
			dynMap.LoadContent(content, world, this);

			boss = dynMap.boss;

            mapBack.LoadContent(content);
            enemies = dynMap.listEnnemy;
            CollectibleItem item = new CollectibleItem();
            /*item.LoadContent(world, new Vector2(1, 1), new Vector2(21, 8), content, CollectibleItemType.AMMO);
            collectibleItems.Add(item);*/

			lifeBarFrame.LoadContent(content, "Graphics/cadre", Color.White, Vector2.Zero);
			lifeBar.LoadContent(content, "Graphics/Lifebar2", Color.White, Vector2.Zero);

            projectileBarFrame.LoadContent(content, "Graphics/cadre", Color.White, Vector2.Zero);
            projectileBar.LoadContent(content, "Graphics/Lifebar2", Color.Purple, Vector2.Zero);
        }

        bool onBeginContact(Contact contact)
        {
            BeginContactForProjectile(contact);
            BeginContactForCollectibleItem(contact);
            BeginContactForPlayer(contact);
            BeginContactForSpike(contact);
            BeginContactForDagger(contact);
            BeginContactForEnemy(contact);
			BeginContactForBoss(contact);

			Fixture fa = contact.FixtureA;
			Fixture fb = contact.FixtureB;

			if ((int)fa.UserData == 9000 && ((int)fb.UserData == 0 || (int)fb.UserData == 1))
			{
				return false;
			}

			if ((int)fb.UserData == 9000 && ((int)fa.UserData == 0 || (int)fb.UserData == 1))
			{
				return false;
			}

            return true;
        }

        void onEndContact(Contact contact)
        {
            EndContactForPlayer(contact);

            EndContactForEnemy(contact);
        }

		//here
		public void BeginContactForBoss(Contact contact)
		{
			Fixture fa = contact.FixtureA;
			Fixture fb = contact.FixtureB;

			if ((int)fa.UserData == 11000 && (int)fb.UserData == 0)
			{
				player.Damage(25);
			}

			if ((int)fb.UserData == 11000 && (int)fa.UserData == 0)
			{
				player.Damage(25);
			}

			if ((int)fa.UserData == 10000
				&& ((int)fb.UserData == 1500
				|| ((int)fb.UserData >= 1000 && (int)fb.UserData < 1100)))
			{
				boss.health -= 30;
				if (boss.health <= 0)
				{
					boss.isDead = true;
					player.isMessage = true;
					player.message = Player.GAGNE;
				}
			}

			if ((int)fb.UserData == 10000
				&& ((int)fa.UserData == 1500
				|| ((int)fa.UserData >= 1000 && (int)fa.UserData < 1100)))
			{
				boss.health -= 30;
				if (boss.health <= 0)
				{
					boss.isDead = true;
					player.isMessage = true;
					player.message = Player.GAGNE;
				}
			}

			if ((int)fa.UserData == 10000
				&& ((int)fb.UserData >= 1000 && (int)fb.UserData < 1100))
			{
				int projectileId = (int)fb.UserData - 1000;
				Projectile proj = projectiles.FirstOrDefault(p => p.id == projectileId);

				if (proj == null)
				{
					return;
				}

				proj.body.Dispose();
				projectiles.Remove(proj);
			}

			if ((int)fb.UserData == 10000
				&& ((int)fa.UserData >= 1000 && (int)fa.UserData < 1100))
			{
				int projectileId = (int)fa.UserData - 1000;
				Projectile proj = projectiles.FirstOrDefault(p => p.id == projectileId);

				if (proj == null)
				{
					return;
				}

				proj.body.Dispose();
				projectiles.Remove(proj);
			}
		}
		public void BeginContactForDagger(Contact contact)
		{
			Fixture fa = contact.FixtureA;
			Fixture fb = contact.FixtureB;

			if ((int)fa.UserData == 1500)
			{
				if ((int)fb.UserData >= 100 && (int)fb.UserData < 200)
				{
					if (player.Health > 0 && player.isAttacking)
					{
						int enemyId = (int)fb.UserData - 100;
						Enemy enemy = enemies.FirstOrDefault(e => e.id == enemyId);
						if (enemy == null)
						{
							return;
						}
						if (!enemy.isInvul)
						{
							enemy.Damage(10);
						}
						if (enemy.Health <= 0)
						{
							enemy.body.Dispose();
							enemies.Remove(enemy);
						}
					}
				}
			}
			if ((int)fb.UserData == 1500)
			{
				if ((int)fa.UserData >= 100 && (int)fa.UserData < 200)
				{
					if (player.Health > 0 && player.isAttacking)
					{
						int enemyId = (int)fb.UserData - 100;
						Enemy enemy = enemies.FirstOrDefault(e => e.id == enemyId);
						if (enemy == null)
						{
							return;
						}
						if (!enemy.isInvul)
						{
							enemy.Damage(10);
						}
						if (enemy.Health <= 0)
						{
							enemy.body.Dispose();
							enemies.Remove(enemy);
						}
					}
				}
			}
		}


        private void BeginContactForSpike(Contact contact)
        {
            Fixture fixtureA = contact.FixtureA;
            Fixture fixtureB = contact.FixtureB;

            if ((int)fixtureA.UserData == 2000)
            {
                //Cas player
                if ((int)fixtureB.UserData == 0)
                {
                    player.Damage(25);
                }
                //cas enemy
                else if ((int)fixtureB.UserData >= 100 && (int)fixtureB.UserData < 200)
                {
                    int enemyId = (int)fixtureB.UserData - 100;
                    Enemy enemy = enemies.FirstOrDefault(e => e.id == enemyId);
                    if (enemy == null)
                    {
                        return;
                    }
                    enemy.Damage(25);
                    if (enemy.Health <= 0)
                    {
                        enemy.body.Dispose();
                        enemies.Remove(enemy);
                    }
                }
            }
            else if ((int)fixtureB.UserData == 2000)
            {
                //cas player
                if ((int)fixtureA.UserData == 0)
                {
                    player.Damage(25);
                }
                //cas enemy
                if ((int)fixtureA.UserData >= 100 && (int)fixtureA.UserData < 200)
                {
                    int enemyId = (int)fixtureA.UserData - 100;
                    Enemy enemy = enemies.FirstOrDefault(e => e.id == enemyId);
                    if (enemy == null)
                    {
                        return;
                    }
                    enemy.Damage(25);
                    if (enemy.Health <= 0)
                    {
                        enemy.body.Dispose();
                        enemies.Remove(enemy);
                    }
                }
            }
        }

        private void BeginContactForProjectile(Contact contact)
        {
            Fixture fixtureA = contact.FixtureA;
            Fixture fixtureB = contact.FixtureB;

            if ((int)fixtureA.UserData >= 1000 && (int)fixtureA.UserData < 1100)
            {
                int projectileId = (int)fixtureA.UserData - 1000;
                Projectile proj = projectiles.FirstOrDefault(p => p.id == projectileId);

                if (proj == null)
                {
                    return;
                }

                int userDataFixtureB = (int)fixtureB.UserData;

                if ((int)fixtureB.UserData == 2)
                {
                    //Mur
                    proj.body.Dispose();
                    projectiles.Remove(proj);
                }
                else if ((int)fixtureB.UserData == 3)
                {
                    //pf
                    proj.body.Dispose();
                    projectiles.Remove(proj);
                }
                else if ((int)fixtureB.UserData >= 100 && (int)fixtureB.UserData < 200)
                {
                    //enemy
                    int enemyId = (int)fixtureB.UserData - 100;
                    Enemy enemy = enemies.FirstOrDefault(e => e.id == enemyId);
                    if (enemy == null)
                    {
                        return;
                    }
                    proj.body.Dispose();
                    projectiles.Remove(proj);
                    enemy.Damage(10);
                    if (enemy.Health <= 0)
                    {
                        enemy.body.Dispose();
                        enemies.Remove(enemy);
                    }


                }

            }
            if ((int)fixtureB.UserData >= 1000 && (int)fixtureB.UserData < 1100)
            {
                int projectileId = (int)fixtureB.UserData - 1000;
                Projectile proj = projectiles.FirstOrDefault(p => p.id == projectileId);

                
                if (proj == null)
                {
                    return;
                }

                int userDataFixtureA = (int)fixtureA.UserData;

                if ((int)fixtureA.UserData == 2)
                {
                    //Mur
                    proj.body.Dispose();
                    projectiles.Remove(proj);
                }
                else if ((int)fixtureA.UserData == 3)
                {
                    //pf
                    proj.body.Dispose();
                    projectiles.Remove(proj);
                }
                else if ((int)fixtureA.UserData >= 100 && (int)fixtureA.UserData < 200)
                {
                    //enemy
                    int enemyId = (int)fixtureA.UserData - 100;
                    Enemy enemy = enemies.FirstOrDefault(e => e.id == enemyId);
                    if (enemy == null)
                    {
                        return;
                    }
                    proj.body.Dispose();
                    projectiles.Remove(proj);
					enemy.Damage(10);
                    if (enemy.Health <= 0)
                    {
                        enemy.body.Dispose();
                        enemies.Remove(enemy);
                    }

                }
            }

        }

        private void BeginContactForCollectibleItem(Contact contact)
        {
            Fixture fixtureA = contact.FixtureA;
            Fixture fixtureB = contact.FixtureB;

            if ((int)fixtureA.UserData >= 500 && (int)fixtureA.UserData < 600
                && (int)fixtureB.UserData == 0)
            {
                int collectibleItemId = (int)fixtureA.UserData - 500;
                CollectibleItem item = collectibleItems
                    .FirstOrDefault(i => i.id == collectibleItemId);
                if (item == null)
                {
                    return;
                }

                if (item.type == CollectibleItemType.HEALTH)
                {
                    //Ajouter santé player ici
                    if (player.Health < 100)
                    {
                        player.Health += 20;
                        item.body.Dispose();
                        collectibleItems.Remove(item);
                        return;
                    }

                }
                if (item.type == CollectibleItemType.AMMO)
                {
                    //Ajouter munition ici
                    player.Ammo += 10;

                    if (player.Ammo >= 100) player.Ammo = 100;

                    item.body.Dispose();
                    collectibleItems.Remove(item);
                    return;
                }
            }
            if ((int)fixtureB.UserData >= 500 && (int)fixtureB.UserData < 600
                && (int)fixtureA.UserData == 0)
            {
                int collectibleItemId = (int)fixtureB.UserData - 500;
                CollectibleItem item = collectibleItems
                    .FirstOrDefault(i => i.id == collectibleItemId);
                if (item == null)
                {
                    return;
                }


                if (item.type == CollectibleItemType.HEALTH)
                {
                    //Ajouter santé player ici
                    if (player.Health < 100)
                    {
                        player.Health += 20;
                        item.body.Dispose();
                        collectibleItems.Remove(item);
                        return;
                    }

                }
                if (item.type == CollectibleItemType.AMMO)
                {
                    //Ajouter munition ici
                    player.Ammo += 10;
                    if (player.Ammo >= 100) player.Ammo = 100;
                    item.body.Dispose();
                    collectibleItems.Remove(item);
                    return;
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
                return;
            }
            if ((int)fixtureB.UserData == 1
                && ((int)fixtureA.UserData == 2
                || (int)fixtureA.UserData == 3))
            {
                player.contactsWithFloor++;
                return;
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
                return;
            }

            if ((int)fixtureB.UserData == 1
                && ((int)fixtureA.UserData == 2
                || (int)fixtureA.UserData == 3))
            {
                player.contactsWithFloor--;
                return;
            }
            if ((int)fixtureA.UserData == 0 && (int)fixtureB.UserData >=100 && (int)fixtureB.UserData < 200)
            {
                player.Damage(10);
            }
            if ((int)fixtureB.UserData == 0 && (int)fixtureA.UserData >= 100 && (int)fixtureA.UserData < 200)
            {
                player.Damage(10);
            }
        }

        private void BeginContactForEnemy(Contact contact)
        {
            Fixture fixtureA = contact.FixtureA;
            Fixture fixtureB = contact.FixtureB;

            int id = 0;

            if ((int)fixtureA.UserData <= -100
                && ((int)fixtureB.UserData == 2
                || (int)fixtureB.UserData == 3))
            {
                id = (int)fixtureA.UserData;
            }
            else if ((int)fixtureB.UserData <= -100
                && ((int)fixtureA.UserData == 2
                || (int)fixtureA.UserData == 3))
            {
                id = (int)fixtureB.UserData;
            }
            else
            {
                return;
            }

            int enemyID = (-id) % 100;
            int sensorID = ((-id) / 100) - 1;

            Enemy enemy = enemies
                .FirstOrDefault(i => i.id == enemyID);
            if (enemy == null)
            {
                return;
            }

			if (enemy == null)
			{
				return;
			}

            enemy.sensorsContacts[sensorID]++;
        }

        private void EndContactForEnemy(Contact contact)
        {
            Fixture fixtureA = contact.FixtureA;
            Fixture fixtureB = contact.FixtureB;

            int id = 0;

            if ((int)fixtureA.UserData <= -100
                && ((int)fixtureB.UserData == 2
                || (int)fixtureB.UserData == 3))
            {
                id = (int)fixtureA.UserData;
            }
            else if ((int)fixtureB.UserData <= -100
                && ((int)fixtureA.UserData == 2
                || (int)fixtureA.UserData == 3))
            {
                id = (int)fixtureB.UserData;
            }
            else
            {
                return;
            }

            int enemyID = (-id) % 100;
            int sensorID = ((-id) / 100) - 1;

            Enemy enemy = enemies
                .FirstOrDefault(i => i.id == enemyID);
			if (enemy == null)
			{
				return;
			}

            enemy.sensorsContacts[sensorID]--;
        }

        void onPreSolve(Contact contact, ref Manifold oldManifold)
        {
            // ...
        }
        void onPostSolve(Contact contact, ContactVelocityConstraint impulse)
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
            dynMap.Update(gameTime);

            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameTime);
            }

            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime, world);
            }

            player.Update(gameTime, world);
			camera.CenterOn(ConvertUnits.ToDisplayUnits(player.GetPositionFromBody()), map);

			lifeBar.position = new Vector2(camera.ScreenToWorld(new Vector2(graphicDevice.Viewport.Width / 2, 0)).X , camera.ScreenToWorld(new Vector2(0, 50)).Y);
			lifeBar.scale = new Vector2(player.Health / 125f, 0.25f);

            projectileBar.position = new Vector2(camera.ScreenToWorld(new Vector2(graphicDevice.Viewport.Width / 2, 0)).X, camera.ScreenToWorld(new Vector2(0, 90)).Y);
            projectileBar.scale = new Vector2(player.Ammo / 125f, 0.25f);

			lifeBarFrame.position = new Vector2(camera.ScreenToWorld(new Vector2(graphicDevice.Viewport.Width / 2, 0)).X - 1, camera.ScreenToWorld(new Vector2(0, 50)).Y - 1);
			lifeBarFrame.scale = new Vector2(0.31f, 0.099f);

            projectileBarFrame.position = new Vector2(camera.ScreenToWorld(new Vector2(graphicDevice.Viewport.Width / 2, 0)).X - 1, camera.ScreenToWorld(new Vector2(0, 90)).Y - 1);
            projectileBarFrame.scale = new Vector2(0.31f, 0.099f);

            // variable time step but never less then 30 Hz
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / PhysicsUtils.FPS)));
        }
        
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.TranslationMatrix);
            mapBack.Draw(spriteBatch);

            map.Draw(spriteBatch);
            dynMap.Draw(spriteBatch);
            foreach (CollectibleItem item in collectibleItems)
            {
                item.Draw(spriteBatch);
            }

            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

			lifeBar.Draw(spriteBatch);
			lifeBarFrame.Draw(spriteBatch);

            projectileBar.Draw(spriteBatch);
            projectileBarFrame.Draw(spriteBatch);

            Matrix cameraMatrix = camera.DebugMatrix;

            //debugView.RenderDebugData(ref projection, ref cameraMatrix);
        }
    }
}
