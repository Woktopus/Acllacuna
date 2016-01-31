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
		const string ritual1 = " inti ";
		const string ritual2 = " killa ";
		const string ritual3 = " pitina ";

        private PhysicsScene physicsScene;

        public Body body;

        public Dagger dagger;

        public Fixture[] feet;

		public Fixture[] bumpers;

		public Animation animation;

        public Vector2 size;

		public Vector2 sizeRatio;

        public int contactsWithFloor;

        public float desiredHorizontalVelocity;

        public bool hasJumped;

        public bool hasMoved;

        public float projectileCooldown;

        //FireRate
        const float SECONDS_IN_MINUTE = 60f;
        const float RATE_OF_FIRE = 200f;
        TimeSpan laserSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / RATE_OF_FIRE);
        TimeSpan previousLaserSpawnTime = TimeSpan.Zero;

        //Attackcooldown
        const float RATE_OF_HIT = 300f;
        TimeSpan attackSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / RATE_OF_HIT);
        TimeSpan previousHitSpawnTime = TimeSpan.Zero;

		const float ATTACK_DURATION = 800f;
		float attackDurationTimer = 0;

		public const float INVUL_DURATION = 1000f;
		public float invulDurationTimer = 0;

        //Stats
        public DirectionEnum directionRegard { get; set; }
        public int Health { get; set; }

		public bool isDamaged;

		public bool isInvul;

        public bool isAttacking;

        public int Ammo { get; set; }

		public Text ritual;

        public Player()
        {
            animation = new Animation();

            dagger = new Dagger();
            dagger.player = this;

            contactsWithFloor = 0;

            Health = 100;
            Ammo = 100;

            feet = new Fixture[3];

			bumpers = new Fixture[10];

			hasJumped = false;
            hasMoved = false;

            projectileCooldown = 0f;

			isDamaged = false;
			isInvul = false;

			isAttacking = false;

			ritual = new Text();
        }



		public void Damage(int damage)
		{
			Health -= damage;
			isDamaged = true;
			isInvul = true;
            if (Health <=0)
            {
                body.Dispose();
				ritual.text = "GAME OVER";
            }
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

		public virtual void SetSize()
		{
			this.size = new Vector2(2.5f, 3f);
			this.sizeRatio = new Vector2(0.7f, 0.9f);
		}

		public virtual void SetIDS()
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

		public virtual void LoadAnimation(ContentManager content)
		{
			animation.LoadContent(content, "Graphics/Spritesheet", Color.White, GetDrawPosition(), 150, new Vector2(4, 5));

			animation.SelectAnimation(0);

			animation.ScaleToMeters(size);

			animation.isActive = true;

			ritual.LoadContent(content, "Graphics/Font/aztec", Color.Red, "", ConvertUnits.ToDisplayUnits(body.Position + new Vector2(0, -size.Y / 2)));
		}

		public void Update(GameTime gameTime, World world)
		{
            if (Health > 0)
            {
                if (isAttacking)
                {
                    attackDurationTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (attackDurationTimer > ATTACK_DURATION)
                    {
                        attackDurationTimer = 0;
                        isAttacking = false;
                        animation.SelectAnimation(0);
                    }
                }

                feet[0].Friction = 1000;
                feet[1].Friction = 1000;
                feet[2].Friction = 1000;

                bumpers[1].Friction = 1000;
                bumpers[4].Friction = 1000;

                SetVelocity(world, gameTime);

                if (hasMoved)
                {
                    feet[0].Friction = 0;
                    feet[1].Friction = 0;
                    feet[2].Friction = 0;

                    bumpers[1].Friction = 0;
                    bumpers[4].Friction = 0;
                }

                for (ContactEdge contactEdge = body.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
                {
                    contactEdge.Contact.ResetFriction();
                }

                if (isDamaged)
                {
					isDamaged = false;
                    float impulse;

                    if (directionRegard == DirectionEnum.LEFT)
                    {
                        float velocityChange = 16 - body.LinearVelocity.X;
                        impulse = body.Mass * velocityChange;
                    }
                    else
                    {
                        float velocityChange = -16 - body.LinearVelocity.X;
                        impulse = body.Mass * velocityChange;
                    }

                    body.ApplyLinearImpulse(new Vector2(impulse, 0), body.WorldCenter);
                    float jumpVelocity = PhysicsUtils.GetVerticalSpeedToReach(world, 2);
                    body.LinearVelocity = new Vector2(body.LinearVelocity.X, -jumpVelocity);

                    animation.SelectAnimation(4);
					animation.loop = false;
                }
                else if (isAttacking)
                {
                    animation.SelectAnimation(3);
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

                animation.position = GetDrawPosition();
                animation.Update(gameTime);

				if (isAttacking)
				{
					if (this.directionRegard == DirectionEnum.LEFT)
					{
						dagger.bodyPosition = new Vector2(this.GetPositionFromBody().X - 1.5f, this.GetPositionFromBody().Y);
						dagger.direc = DirectionEnum.LEFT;
					}
					else
					{
						dagger.bodyPosition = new Vector2(this.GetPositionFromBody().X + 1.5f, this.GetPositionFromBody().Y);
						dagger.direc = DirectionEnum.RIGHT;
					}
				}
				else
				{
					dagger.bodyPosition = Vector2.Zero;
				}
                dagger.Update(gameTime); 
            }

            dagger.Update(gameTime);

			ritual.position = ConvertUnits.ToDisplayUnits(body.Position + new Vector2(0, -size.Y / 2));

			if (isInvul)
			{
				if (animation.textureColor == Color.White)
				{
					animation.textureColor = Color.Pink;
				}
				else
				{
					animation.textureColor = Color.White;
				}

				invulDurationTimer += gameTime.ElapsedGameTime.Milliseconds;
				if (invulDurationTimer > INVUL_DURATION)
				{
					invulDurationTimer = 0;
					isInvul = false;
					animation.textureColor = Color.White;
				}
			}
        }

		public void UpdateRitual()
		{
			KeyboardState keyboardInput = ServiceHelper.Get<InputManagerService>().Keyboard.GetState();
			KeyboardState prevInput = ServiceHelper.Get<InputManagerService>().Keyboard.GetPrevState();

			if (keyboardInput.IsKeyDown(Keys.A))
			{
				if (prevInput.IsKeyDown(Keys.D1) && keyboardInput.IsKeyUp(Keys.D1))
				{
					ritual.text += ritual1;
					return;
				}
				if (prevInput.IsKeyDown(Keys.D2) && keyboardInput.IsKeyUp(Keys.D2))
				{
					ritual.text += ritual2;
					return;
				}
				if (prevInput.IsKeyDown(Keys.D3) && keyboardInput.IsKeyUp(Keys.D3))
				{
					ritual.text += ritual3;
					return;
				}
			}
			else if (prevInput.IsKeyDown(Keys.A))
			{
				if (ritual.text == ritual1 + ritual2 + ritual3)
				{
					if (Ammo >= 50)
					{
						Ammo -= 50;
						Health = 100;
					}
				}
				ritual.text = "";
			}
		}

        public virtual void SetVelocity(World world, GameTime gameTime)
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
					this.isAttacking = true;
                }
			}

			UpdateRitual();
        }

        public void LaunchProjectile()
        {
            this.physicsScene.projectileFactory.LaunchProjectile(this.directionRegard,
            new Vector2(1, 1), body.Position, "Graphics/Projectile/couteau", 10);
            Ammo--;
        }

		public void Draw(SpriteBatch spriteBatch)
		{
            if (Health >0)
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
			ritual.Draw(spriteBatch);
		}

        public Vector2 GetDrawPosition()
        {
            return ConvertUnits.ToDisplayUnits(body.Position);
        }
    }
}
