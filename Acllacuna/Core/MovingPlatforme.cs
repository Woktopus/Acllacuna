using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics;

namespace Acllacuna
{
    public class MovingPlatforme
    {

        public Body body { get; set; }
        public Image image { get; set; }



        public PlatformeDirection pattern { get; set; }


        public Vector2 speedDirection { get; set; }
        public float dist { get; set; }
        public float maxDist { get; set; }
        public float speed { get; set; }


        public MovingPlatforme()
        {
        }

        public void LoadContent(World world, Vector2 size, Vector2 position, ContentManager Content, string texturePath, PlatformeDirection pattern,  float maxDist, float speed)
        {
            //Initialisation du body 
            body = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);
            body.BodyType = BodyType.Kinematic;
            body.Position = position;

            //Initialisation de l'image du block
            Vector2 imagePosition = new Vector2(ConvertUnits.ToDisplayUnits(position.X), ConvertUnits.ToDisplayUnits(position.Y));
            image = new Image();
            image.LoadContent(Content, texturePath, Color.White, imagePosition);
            image.ScaleToMeters(size);

            this.pattern = pattern;
            this.dist = 0;
            this.maxDist = maxDist;
            this.speedDirection = speedDirection;

            this.speed = speed;

            if (pattern == PlatformeDirection.RIGHT_LEFT)
            {
                this.speedDirection = new Vector2(1, 0);
            }
            else if (pattern == PlatformeDirection.LEFT_RIGHT)
            {
                this.speedDirection = new Vector2(-1, 0);
            }
            else if (pattern == PlatformeDirection.UP_DOWN)
            {
                this.speedDirection = new Vector2(0, 1);
            }
            else if (pattern == PlatformeDirection.DOWN_UP)
            {
                this.speedDirection = new Vector2(0, -1);
            }
            this.speedDirection = speedDirection * speed;
        }

        public void Update(GameTime gameTime)
        {
            dist += (speedDirection.Length() * (float)gameTime.ElapsedGameTime.Milliseconds)/1000;
            if (dist >= maxDist)
            {
                if (pattern == PlatformeDirection.RIGHT_LEFT)
                {
                    this.pattern = PlatformeDirection.LEFT_RIGHT;
                    this.speedDirection = new Vector2(-1, 0);
                }
                else if (pattern == PlatformeDirection.LEFT_RIGHT)
                {
                    this.pattern = PlatformeDirection.RIGHT_LEFT;
                    this.speedDirection = new Vector2(1, 0);
                }
                else if (pattern == PlatformeDirection.UP_DOWN)
                {
                    pattern = PlatformeDirection.DOWN_UP;
                    this.speedDirection = new Vector2(0, -1);
                }
                else if (pattern == PlatformeDirection.DOWN_UP)
                {
                    pattern = PlatformeDirection.UP_DOWN;
                    this.speedDirection = new Vector2(0, 1);
                }
                this.speedDirection = speedDirection * speed;
                dist = 0;
            }
            body.LinearVelocity = speedDirection;
            image.position = ConvertUnits.ToDisplayUnits(body.Position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch);
        }
    }
}
