using FarseerPhysics;
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
    public class Projectile
    {
        private static int NextId = 0;

        public int id { get; set; }
        public Body body { get; set; }
        public Vector2 bodySize { get; set; }
        public Image image { get; set; }
        public DirectionEnum direction { get; set; }
        public Vector2 speedDirection { get; set; }
        public float speed { get; set; }



        public Projectile()
        {
            this.id = NextId;
            NextId++;
        }

        public void LoadContent(World world, Vector2 size, Vector2 position, ContentManager Content, string texturePath, DirectionEnum direction, float speed)
        {
            //Initialisation du body
            body = BodyFactory.CreateRectangle(world, size.X, size.Y, 1f);
            body.FixtureList[0].UserData = (int)(1000+id);
            body.BodyType = BodyType.Dynamic;
            body.Position = position;
            this.bodySize = size;
            body.FixtureList[0].IsSensor = true;

            //Initialisation de l'image du block
            Vector2 imagePosition = new Vector2(ConvertUnits.ToDisplayUnits(position.X), ConvertUnits.ToDisplayUnits(position.Y));
            image = new Image();
            image.LoadContent(Content, texturePath, Color.Purple, imagePosition);
            image.ScaleToMeters(size);

            if (direction == DirectionEnum.LEFT)
            {
                this.speedDirection = new Vector2(-1,0);
            }
            else
            {
                this.speedDirection = new Vector2(1, 0);
            }
            this.direction = direction;
           
            this.speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            body.ApplyForce(-PhysicsUtils.gravity);
            if (direction == DirectionEnum.RIGHT)
            {
                Vector2 impulse = new Vector2(1,0)* speed;
                body.LinearVelocity = impulse;
                image.position = ConvertUnits.ToDisplayUnits(body.Position);
            }
            else
            {
                Vector2 impulse = new Vector2(-1, 0) * speed;
                body.LinearVelocity = impulse;
                image.position = ConvertUnits.ToDisplayUnits(body.Position);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (direction == DirectionEnum.RIGHT)
            {
                 image.Draw(spriteBatch);
            }
            else
            {
                image.DrawFlipHorizontally(spriteBatch);
            }
        }

    }
}
