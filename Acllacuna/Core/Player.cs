using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Acllacuna
{
	public class Player
	{
        private PhysicsScene physicsScene;

        public Body body;

        public Dagger dagger;

        protected Fixture[] feet;

		protected Fixture[] bumpers;

		protected Animation animation;

        protected Vector2 size;

		protected Vector2 sizeRatio;

        public int contactsWithFloor;

        protected float desiredHorizontalVelocity;

        protected bool hasJumped;

        protected bool hasMoved;

        public float projectileCooldown;

        //FireRate
        const float SECONDS_IN_MINUTE = 60f;
        const float RATE_OF_FIRE = 200f;
        TimeSpan laserSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / RATE_OF_FIRE);
        TimeSpan previousLaserSpawnTime = TimeSpan.Zero;

        //Attackcooldown
        const float RATE_OF_HIT = 150f;
        TimeSpan attackSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / RATE_OF_HIT);
        TimeSpan previousHitSpawnTime = TimeSpan.Zero;

        //Stats
        public DirectionEnum directionRegard { get; set; }
        public int Health { get; set; }

		public bool isDamaged;

        public bool isAttacking;

        public int Ammo { get; set; }

        public Player()
        {
            animation = new Animation();

            dagger = new Dagger();

            contactsWithFloor = 0;

            Health = 100;
            Ammo = 100;

            feet = new Fixture[3];

			bumpers = new Fixture[10];

			hasJumped = false;
            hasMoved = false;

            projectileCooldown = 0f;

			isDamaged = false;
        }



		public void Damage(int damage)
		{
			Health -= damage;
			isDamaged = true;
		}

        public Vector2 GetPositionFromBody()
        {
            return body.Position;
        }

        public virtual void LoadContent(World world, ContentManager content, Vector2 position, PhysicsScene physicsScene)
		{
			SetSize();

            this.physicsScene = physicsScene;

			Vector2 size = this.size * sizeRatio;

            body = BodyFactory.CreateRectangle(world, size.X, size.Y - 0.1f, 1f);

            body.BodyType = BodyType.Dynamic;

            body.FixedRotation = true;

            body.Position = position;

            body.Mass = 50;

            dagger.LoadContent(content, world, new Vector2(1, 1), this.GetPositionFromBody(), "Graphics/lame_hitbox");

            CircleShape circle1 = new CircleShape(0.1f, 1f);
            CircleShape circle2 = new CircleShape(0.1f, 1f);
            CircleShape circle3 = new CircleShape(0.1f, 1f);
            CircleShape circle4 = new CircleShape(0.1f, 1f);
            CircleShape circle5 = new CircleShape(0.1f, 1f);
			CircleShape circle6 = new CircleShape(0.1f, 1f);
			CircleShape circle7 = new CircleShape(0.1f, 1f);
			CircleShape circle8 = new CircleShape(0.1f, 1f);
			CircleShape circle9 = new CircleShape(0.1f, 1f);
			CircleShape circle10 = new CircleShape(0.1f, 1f);
			CircleShape circle11 = new CircleShape(0.1f, 1f);
			CircleShape circle12 = new CircleShape(0.1f, 1f);
			CircleShape circle13 = new CircleShape(0.1f, 1f);

            circle1.Position = new Vector2(-((size.X / 2) - 0.2f), (size.Y - 0.1f) / 2);
            circle2.Position = new Vector2(0, (size.Y - 0.1f) / 2);
            circle3.Position = new Vector2((size.X / 2) - 0.2f, (size.Y - 0.1f) / 2);

			circle4.Position = new Vector2(-size.X / 2, -(size.Y - 0.1f) / 2);
			circle5.Position = new Vector2(-size.X / 2, (size.Y - 0.1f) / 2);
			circle6.Position = new Vector2(-size.X / 2, 0);
			circle7.Position = new Vector2(size.X / 2, -(size.Y - 0.1f) / 2);
			circle8.Position = new Vector2(size.X / 2, (size.Y - 0.1f) / 2);
			circle9.Position = new Vector2(size.X / 2, 0);
			circle10.Position = new Vector2(size.X / 2, -(size.Y - 0.1f) / 4);
			circle11.Position = new Vector2(size.X / 2, (size.Y - 0.1f) / 4);
			circle12.Position = new Vector2(-size.X / 2, -(size.Y - 0.1f) / 4);
			circle13.Position = new Vector2(-size.X / 2, (size.Y - 0.1f) / 4);

			feet[0] = body.CreateFixture(circle1);
			feet[1] = body.CreateFixture(circle2);
			feet[2] = body.CreateFixture(circle3);

			bumpers[0] = body.CreateFixture(circle4);
			bumpers[1] = body.CreateFixture(circle5);
			bumpers[2] = body.CreateFixture(circle6);
			bumpers[3] = body.CreateFixture(circle7);
			bumpers[4] = body.CreateFixture(circle8);
			bumpers[5] = body.CreateFixture(circle9);
			bumpers[6] = body.CreateFixture(circle10);
			bumpers[7] = body.CreateFixture(circle11);
			bumpers[8] = body.CreateFixture(circle12);
			bumpers[9] = body.CreateFixture(circle13);

            directionRegard = DirectionEnum.RIGHT;

			SetIDS();

			LoadAnimation(content);
		}

		protected virtual void SetSize()
		{
			this.size = new Vector2(2.5f, 3f);
			this.sizeRatio = new Vector2(0.7f, 0.9f);
		}

		protected virtual void SetIDS()
		{
			body.FixtureList[0].UserData = (int)0;
			feet[0].UserData = (int)1;
			feet[1].UserData = (int)1;
			feet[2].UserData = (int)1;
			bumpers[0].UserData = (int)0;
			bumpers[1].UserData = (int)0;
			bumpers[2].UserData = (int)0;
			bumpers[3].UserData = (int)0;
			bumpers[4].UserData = (int)0;
			bumpers[5].UserData = (int)0;
			bumpers[6].UserData = (int)0;
			bumpers[7].UserData = (int)0;
			bumpers[8].UserData = (int)0;
			bumpers[9].UserData = (int)0;
		}

		protected virtual void LoadAnimation(ContentManager content)
		{
			animation.LoadContent(content, "Graphics/Spritesheet", Color.White, GetDrawPosition(), 150, new Vector2(4, 5));

			animation.SelectAnimation(0);

			animation.ScaleToMeters(size);

			animation.isActive = true;
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

				animation.SelectAnimation(4);
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
           
            if(this.directionRegard == DirectionEnum.LEFT){
                dagger.bodyPosition = new Vector2(this.GetPositionFromBody().X - 1.5f, this.GetPositionFromBody().Y);
                dagger.direc = DirectionEnum.LEFT;
            }
            else
            {
                dagger.bodyPosition = new Vector2(this.GetPositionFromBody().X + 1.5f, this.GetPositionFromBody().Y);
                dagger.direc = DirectionEnum.RIGHT;
            }
            dagger.Update(gameTime);


        }

        protected virtual void SetVelocity(World world, GameTime gameTime)
		{
			KeyboardState keyboardInput = ServiceHelper.Get<InputManagerService>().Keyboard.GetState();

			Vector2 velocity = body.LinearVelocity;
			
			hasMoved = false;

			desiredHorizontalVelocity = velocity.X * 0.95f;
			if (keyboardInput.IsKeyDown(Keys.Left))
			{
				desiredHorizontalVelocity = MathHelper.Max(velocity.X - 0.5f, -5.0f);
				hasMoved = true;
                this.directionRegard = DirectionEnum.LEFT;
            }
			if (keyboardInput.IsKeyDown(Keys.Right))
            {
                desiredHorizontalVelocity = MathHelper.Min(velocity.X + 0.5f, 5.0f);
                hasMoved = true;
                this.directionRegard = DirectionEnum.RIGHT;
            }

            float velocityChange = desiredHorizontalVelocity - velocity.X;
            float impulse = body.Mass * velocityChange;

            body.ApplyLinearImpulse(new Vector2(impulse, 0), body.WorldCenter);

			if (keyboardInput.IsKeyDown(Keys.Up) && contactsWithFloor > 0)
			{
				float jumpVelocity = PhysicsUtils.GetVerticalSpeedToReach(world, 3);
				body.LinearVelocity = new Vector2(velocity.X, -jumpVelocity);
				hasJumped = true;
			}

            if (keyboardInput.IsKeyDown(Keys.Space))
            {
                if (Ammo > 0)
                {
                    if (gameTime.TotalGameTime - previousLaserSpawnTime > laserSpawnTime)
                    {
                        previousLaserSpawnTime = gameTime.TotalGameTime;
                        LaunchProjectile();
                    }
                    else
                    {
                        projectileCooldown -= gameTime.ElapsedGameTime.Milliseconds;
                    }
                }
            }

            if ( keyboardInput.IsKeyDown(Keys.C))
            {
                if (gameTime.TotalGameTime - previousHitSpawnTime > attackSpawnTime)
                {
                    previousHitSpawnTime = gameTime.TotalGameTime;
                    Attack();
                }
            }
        }

        public void LaunchProjectile()
        {
            this.physicsScene.projectileFactory.LaunchProjectile(this.directionRegard,
            new Vector2(1, 1), body.Position, "Graphics/Projectile/lame_hitbox", 10);
            Ammo--;
        }

        public void Attack()
        {
            this.isAttacking = true;
        }

		public void Draw(SpriteBatch spriteBatch)
		{
            dagger.Draw(spriteBatch);
			if (directionRegard == DirectionEnum.RIGHT)
			{
				animation.Draw(spriteBatch);
			}
			else
			{
				animation.DrawFlipHorizontally(spriteBatch);
			}
		}

        protected Vector2 GetDrawPosition()
        {
            return ConvertUnits.ToDisplayUnits(body.Position);
        }
    }
}
